﻿/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using Sportradar.MTS.SDK.Common.Internal;
using Sportradar.MTS.SDK.Common.Log;
using Sportradar.MTS.SDK.Entities.EventArguments;

namespace Sportradar.MTS.SDK.API.Internal.RabbitMq
{
    /// <summary>
    /// Implementation of <see cref="IRabbitMqPublisherChannel"/>
    /// </summary>
    /// <seealso cref="IRabbitMqPublisherChannel" />
    public class RabbitMqPublisherChannel : IRabbitMqPublisherChannel
    {
        /// <summary>
        /// Gets the unique identifier
        /// </summary>
        /// <value>The unique identifier</value>
        public int UniqueId { get; }

        /// <summary>
        /// Raised when the attempt to publish message failed
        /// </summary>
        public event EventHandler<MessagePublishFailedEventArgs> MqMessagePublishFailed;

        /// <summary>
        /// The ILog used execution logging
        /// </summary>
        private static readonly ILog ExecutionLog = SdkLoggerFactory.GetLogger(typeof(RabbitMqPublisherChannel));

        /// <summary>
        /// The feed log
        /// </summary>
        private static readonly ILog FeedLog = SdkLoggerFactory.GetLoggerForFeedTraffic(typeof(RabbitMqPublisherChannel));

        /// <summary>
        /// A <see cref="IChannelFactory" /> used to construct the <see cref="IModel" /> representing Rabbit MQ channel
        /// </summary>
        private readonly IChannelFactory _channelFactory;

        /// <summary>
        /// Value indicating whether the current instance is opened. 1 means opened, 0 means closed
        /// </summary>
        private long _isOpened;

        /// <summary>
        /// Value indicating whether the current instance should be opened. 1 means yes, 0 means no
        /// </summary>
        private long _shouldBeOpened;

        /// <summary>
        /// Gets a value indicating whether the current <see cref="RabbitMqMessageReceiver" /> is currently opened;
        /// </summary>
        /// <value><c>true</c> if this instance is opened; otherwise, <c>false</c></value>
        public bool IsOpened => Interlocked.Read(ref _isOpened) == 1;

        /// <summary>
        /// The MTS channel settings
        /// </summary>
        private readonly IMtsChannelSettings _mtsChannelSettings;

        /// <summary>
        /// The channel settings
        /// </summary>
        private readonly IRabbitMqChannelSettings _channelSettings;

        /// <summary>
        /// The use queue
        /// </summary>
        private readonly bool _useQueue;

        /// <summary>
        /// The queue timeout
        /// </summary>
        private readonly int _queueTimeout;

        /// <summary>
        /// The queue limit
        /// </summary>
        private readonly int _queueLimit;

        /// <summary>
        /// The MSG queue
        /// </summary>
        private readonly ConcurrentQueue<PublishQueueItem> _msgQueue;

        /// <summary>
        /// The queue timer
        /// </summary>
        private readonly ITimer _queueTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqConsumerChannel" /> class
        /// </summary>
        /// <param name="channelFactory">A <see cref="IChannelFactory" /> used to construct the <see cref="IModel" /> representing Rabbit MQ channel</param>
        /// <param name="mtsChannelSettings">The mts channel settings</param>
        /// <param name="channelSettings">The channel settings</param>
        public RabbitMqPublisherChannel(IChannelFactory channelFactory, IMtsChannelSettings mtsChannelSettings, IRabbitMqChannelSettings channelSettings)
        {
            Contract.Requires(channelFactory != null);
            Contract.Requires(mtsChannelSettings != null);
            Contract.Requires(channelSettings != null);

            _channelFactory = channelFactory;

            _mtsChannelSettings = mtsChannelSettings;

            _channelSettings = channelSettings;

            _useQueue = false;
            if (channelSettings.PublishQueueTimeoutInMs > 0 || channelSettings.PublishQueueLimit > 0)
            {
                _useQueue = true;
                _msgQueue = new ConcurrentQueue<PublishQueueItem>();
                _queueLimit = channelSettings.PublishQueueLimit > 1 ? _channelSettings.PublishQueueLimit : -1;
                _queueTimeout = channelSettings.PublishQueueTimeoutInMs >= 10000 ? _channelSettings.PublishQueueTimeoutInMs : 15000;
                _queueTimer = new SdkTimer(new TimeSpan(0, 0, 5), new TimeSpan(0, 0, 1));
                _queueTimer.Elapsed += OnTimerElapsed;
                _queueTimer.FireOnce(new TimeSpan(0, 0, 5));
            }
            _shouldBeOpened = 0;

            UniqueId = _channelFactory.GetUniqueId();
        }

        /// <summary>
        /// Defines invariant members of the class
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(ExecutionLog != null);
            Contract.Invariant(_channelFactory != null);
            Contract.Invariant(_channelSettings != null);
        }

        /// <summary>
        /// Invoked when the internally used timer elapses
        /// </summary>
        /// <param name="sender">A <see cref="object" /> representation of the <see cref="ITimer" /> raising the event</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data</param>
        private void OnTimerElapsed(object sender, EventArgs e)
        {
            while (!_msgQueue.IsEmpty)
            {
                PublishQueueItem pqi = null;
                try
                {
                    if (_msgQueue.TryDequeue(out pqi))
                    {
                        //check if expired
                        if (pqi.Timestamp < DateTime.Now.AddMilliseconds(-_queueTimeout))
                        {
                            var msg = $"At {DateTime.Now} publishing queue item is expired. CorrelationId={pqi.CorrelationId}, RoutingKey={pqi.RoutingKey}, Added={pqi.Timestamp}.";
                            ExecutionLog.Error(msg);
                            FeedLog.Error(msg);
                            RaiseMessagePublishFailedEvent(pqi.Message, pqi.CorrelationId, pqi.RoutingKey, "Queue item is expired.");
                            continue;
                        }

                        //publish
                        var publishResult = PublishMsg((byte[]) pqi.Message, pqi.RoutingKey, pqi.CorrelationId, pqi.ReplyRoutingKey);

                        if (publishResult.IsSuccess)
                        {
                            if (FeedLog.IsDebugEnabled)
                            {
                                FeedLog.Debug($"Publish succeeded. CorrelationId={pqi.CorrelationId}, RoutingKey={pqi.RoutingKey}, ReplyRoutingKey={pqi.ReplyRoutingKey}, Added={pqi.Timestamp}.");
                            }
                            else
                            {
                                FeedLog.Info($"Publish succeeded. CorrelationId={pqi.CorrelationId}, RoutingKey={pqi.RoutingKey}, Added={pqi.Timestamp}.");
                            }
                        }
                        else
                        {
                            FeedLog.Warn($"Publish failed. CorrelationId={pqi.CorrelationId}, RoutingKey={pqi.RoutingKey}, Added={pqi.Timestamp}. Reason={publishResult.Message}");
                            RaiseMessagePublishFailedEvent(pqi.Message, pqi.CorrelationId, pqi.RoutingKey, publishResult.Message);
                        }
                    }
                }
                catch (Exception exception)
                {
                    FeedLog.Error($"Error during publishing queue item. CorrelationId={pqi?.CorrelationId}, RoutingKey={pqi?.RoutingKey}, Added={pqi?.Timestamp}.", exception);
                    Contract.Assume(pqi != null);
                    RaiseMessagePublishFailedEvent(pqi.Message, pqi.CorrelationId, pqi.RoutingKey, "Error during publishing queue item: " + exception);
                }
            }

            if (_useQueue)
            {
                _queueTimer.FireOnce(TimeSpan.FromMilliseconds(250)); // recheck after X milliseconds
            }
        }

        /// <summary>
        /// Raises the message publish failed event
        /// </summary>
        /// <param name="rawData">The raw data</param>
        /// <param name="correlationId">The correlation identifier</param>
        /// <param name="routingKey">The routing key</param>
        /// <param name="message">The message</param>
        private void RaiseMessagePublishFailedEvent(IEnumerable<byte> rawData, string correlationId, string routingKey, string message)
        {
            var args = new MessagePublishFailedEventArgs(rawData, correlationId, routingKey, message);
            MqMessagePublishFailed?.Invoke(this, args);
        }

        /// <summary>
        /// Publishes the specified message
        /// </summary>
        /// <param name="msg">The message to be published</param>
        /// <param name="routingKey">The routing key to be set while publishing</param>
        /// <param name="correlationId">The correlation identifier</param>
        /// <param name="replyRoutingKey">The reply routing key</param>
        /// <returns>A <see cref="IMqPublishResult" /></returns>
        /// <exception cref="System.InvalidOperationException">The instance is closed</exception>
        public IMqPublishResult Publish(byte[] msg, string routingKey, string correlationId, string replyRoutingKey)
        {
            if (_shouldBeOpened == 0)
            {
                throw new InvalidOperationException("The instance is closed");
            }
            if (_useQueue)
            {
                return AddToPublishingQueue(msg, routingKey, correlationId, replyRoutingKey);
            }
            return PublishMsg(msg, routingKey, correlationId, replyRoutingKey);
        }

        /// <summary>
        /// Publish message as an asynchronous operation
        /// </summary>
        /// <param name="msg">The message to be published</param>
        /// <param name="routingKey">The routing key to be set while publishing</param>
        /// <param name="correlationId">The correlation identifier</param>
        /// <param name="replyRoutingKey">The reply routing key</param>
        /// <returns>A <see cref="IMqPublishResult" /></returns>
        public async Task<IMqPublishResult> PublishAsync(byte[] msg, string routingKey, string correlationId, string replyRoutingKey)
        {
            return await Task.Run(() => Publish(msg, routingKey, correlationId, replyRoutingKey)).ConfigureAwait(false);
        }

        /// <summary>
        /// Adds to publishing queue
        /// </summary>
        /// <param name="msg">The MSG</param>
        /// <param name="routingKey">The routing key</param>
        /// <param name="correlationId">The correlation identifier</param>
        /// <param name="replyRoutingKey">The reply routing key</param>
        /// <returns>IMqPublishResult</returns>
        private IMqPublishResult AddToPublishingQueue(byte[] msg, string routingKey, string correlationId, string replyRoutingKey)
        {
            var item = new PublishQueueItem(msg, routingKey, correlationId, replyRoutingKey);

            if (_queueLimit > 0 && _msgQueue.Count >= _queueLimit)
            {
                var errorMessage = $"Publishing Queue is full. CorrelationId={correlationId}, RoutingKey={routingKey}.";
                FeedLog.Error(errorMessage);
                ExecutionLog.Error(errorMessage);
                //since user called Publish, we just return result and no need to call event handler
                //var args = new MessagePublishFailedEventArgs(msg, correlationId, routingKey, errorMessage);
                //MqMessagePublishFailed?.Invoke(this, args);
                return new MqPublishResult(correlationId, false, errorMessage);
            }

            _msgQueue.Enqueue(item);
            FeedLog.Debug($"Message with correlationId:{correlationId} and routingKey:{routingKey} added to publishing queue.");
            return new MqPublishResult(correlationId, true, "Item added to publishing queue.");
        }

        /// <summary>
        /// Publishes the MSG
        /// </summary>
        /// <param name="msg">The MSG</param>
        /// <param name="routingKey">The routing key</param>
        /// <param name="correlationId">The correlation identifier</param>
        /// <param name="replyRoutingKey">The reply routing key</param>
        /// <returns>IMqPublishResult</returns>
        /// <exception cref="System.InvalidOperationException">The instance is closed</exception>
        private IMqPublishResult PublishMsg(byte[] msg, string routingKey, string correlationId, string replyRoutingKey)
        {
            try
            {
                var channelWrapper = _channelFactory.GetChannel(UniqueId);
                CreateAndOpenPublisherChannel();
                if (channelWrapper.ChannelBasicProperties == null)
                {
                    throw new OperationCanceledException($"Channel {UniqueId} is not initiated.");
                }
                var channelBasicProperties = channelWrapper.ChannelBasicProperties;
                if (channelBasicProperties.Headers == null)
                {
                    channelBasicProperties.Headers = new Dictionary<string, object>();
                }
                if (!string.IsNullOrEmpty(correlationId))
                {
                    //_channelBasicProperties.Headers["correlationId"] = correlationId;
                    channelBasicProperties.CorrelationId = correlationId;
                }
                if (!string.IsNullOrEmpty(replyRoutingKey))
                {
                    channelBasicProperties.Headers["replyRoutingKey"] = replyRoutingKey;
                }
                channelWrapper.Channel.BasicPublish(_mtsChannelSettings.ExchangeName, routingKey, channelBasicProperties, msg);
                FeedLog.Debug($"Publish of message with correlationId:{correlationId} and routingKey:{routingKey} succeeded.");
                return new MqPublishResult(correlationId);
            }
            catch (Exception e)
            {
                FeedLog.Error($"Publish of message with correlationId:{correlationId} and routingKey:{routingKey} failed.", e);
                return new MqPublishResult(correlationId, false, e.Message);
            }
        }

        private void CreateAndOpenPublisherChannel()
        {
            var sleepTime = 1000;
            while (Interlocked.Read(ref _shouldBeOpened) == 1)
            {
                try
                {
                    var channelWrapper = _channelFactory.GetChannel(UniqueId);
                    if (channelWrapper == null)
                    {
                        throw new OperationCanceledException("Missing publisher channel wrapper.");
                    }
                    if (channelWrapper.MarkedForDeletion)
                    {
                        throw new OperationCanceledException("Publisher channel marked for deletion.");
                    }
                    if (channelWrapper.Channel.IsOpen && channelWrapper.ChannelBasicProperties != null)
                    {
                        return;
                    }
                    channelWrapper.Channel.ModelShutdown += ChannelOnModelShutdown;
                    ExecutionLog.Info($"Opening the publisher channel with channelNumber: {UniqueId} and exchangeName: {_mtsChannelSettings.ExchangeName}.");

                    // try to declare the exchange if it is not the default one
                    if (!string.IsNullOrEmpty(_mtsChannelSettings.ExchangeName))
                    {
                        try
                        {
                            channelWrapper.Channel.ExchangeDeclare(_mtsChannelSettings.ExchangeName,
                                                     _mtsChannelSettings.ExchangeType.ToString().ToLower(),
                                                     _channelSettings.QueueIsDurable,
                                                     false,
                                                     null);
                        }
                        catch (Exception ie)
                        {
                            ExecutionLog.Error(ie.Message, ie);
                            ExecutionLog.Warn($"Exchange {_mtsChannelSettings.ExchangeName} creation failed, will try to recreate it.");
                            channelWrapper.Channel.ExchangeDelete(_mtsChannelSettings.ExchangeName);
                            channelWrapper.Channel.ExchangeDeclare(_mtsChannelSettings.ExchangeName,
                                                     _mtsChannelSettings.ExchangeType.ToString().ToLower(),
                                                     _channelSettings.QueueIsDurable,
                                                     false,
                                                     null);
                        }
                    }

                    var channelBasicProperties = channelWrapper.Channel.CreateBasicProperties();
                    channelBasicProperties.ContentType = "application/json";
                    channelBasicProperties.DeliveryMode = _channelSettings.UsePersistentDeliveryMode ? (byte)2 : (byte)1;

                    //headerProperties like replyRoutingKey
                    channelBasicProperties.Headers = new Dictionary<string, object>();
                    if (_mtsChannelSettings.HeaderProperties != null && _mtsChannelSettings.HeaderProperties.Any())
                    {
                        foreach (var h in _mtsChannelSettings.HeaderProperties)
                        {
                            channelBasicProperties.Headers.Add(h.Key, h.Value);
                        }
                    }

                    channelWrapper.ChannelBasicProperties = channelBasicProperties;

                    Interlocked.CompareExchange(ref _isOpened, 1, 0);
                    //ExecutionLog.Debug($"Opening the publisher channel with channelNumber: {UniqueId} and exchangeName: {_mtsChannelSettings.ExchangeName} finished.");
                    return;
                }
                catch (Exception e)
                {
                    ExecutionLog.Info($"Error opening the publisher channel with channelNumber: {UniqueId} and exchangeName: {_mtsChannelSettings.ExchangeName}.", e);
                    if (e is IOException || e is AlreadyClosedException || e is SocketException)
                    {
                        sleepTime = SdkInfo.Increase(sleepTime, 500, 10000);
                    }
                    else
                    {
                        sleepTime = SdkInfo.Multiply(sleepTime, 1.25, _channelSettings.PublishQueueTimeoutInMs * 1000);
                    }
                    ExecutionLog.Info($"Opening the publisher channel will be retried in next {sleepTime} ms.");
                    Thread.Sleep(sleepTime);
                }
            }
        }

        private void ChannelOnModelShutdown(object sender, ShutdownEventArgs e)
        {
            ExecutionLog.Info($"Shutdown publisher channel with channelNumber: {UniqueId} and exchangeName: {_mtsChannelSettings.ExchangeName}. Reason={e.ReplyCode}-{e.ReplyText}, Cause={e.Cause}");
            Interlocked.CompareExchange(ref _isOpened, 0, 1);
        }

        /// <summary>
        /// Opens the current channel and binds the created queue to provided routing keys
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The instance is already opened</exception>
        public void Open()
        {
            if (Interlocked.Read(ref _isOpened) != 0)
            {
                ExecutionLog.Error("Opening an already opened channel is not allowed");
                throw new InvalidOperationException("The instance is already opened");
            }

            Interlocked.CompareExchange(ref _shouldBeOpened, 1, 0);
            CreateAndOpenPublisherChannel();
        }

        /// <summary>
        /// Closes the current channel
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The instance is already closed</exception>
        public void Close()
        {
            //Contract.Assume(_channel != null);
            if (Interlocked.CompareExchange(ref _isOpened, 0, 1) != 1)
            {
                // Do not show error if the channel is scheduled to be open
                if (Interlocked.Read(ref _shouldBeOpened) != 1)
                    ExecutionLog.Error($"Cannot close the publisher channel on channelNumber: {UniqueId}, because this channel is already closed.");
                //throw new InvalidOperationException("The instance is already closed");
            }
            Interlocked.CompareExchange(ref _shouldBeOpened, 0, 1);
        }
    }
}
