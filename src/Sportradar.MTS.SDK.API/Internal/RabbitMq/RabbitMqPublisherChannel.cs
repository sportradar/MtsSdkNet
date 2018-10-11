/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using RabbitMQ.Client;
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
        /// Raised when the attempt to publish message failed
        /// </summary>
        public event EventHandler<MessagePublishFailedEventArgs> MqMessagePublishFailed;

        /// <summary>
        /// The ILog used execution logging
        /// </summary>
        private static readonly ILog ExecutionLog = SdkLoggerFactory.GetLogger(typeof(RabbitMqPublisherChannel));
        private static readonly ILog FeedLog = SdkLoggerFactory.GetLoggerForFeedTraffic(typeof(RabbitMqPublisherChannel));

        /// <summary>
        /// A <see cref="IChannelFactory" /> used to construct the <see cref="IModel" /> representing Rabbit MQ channel
        /// </summary>
        private readonly IChannelFactory _channelFactory;

        /// <summary>
        /// The <see cref="IModel" /> representing the channel to the broker
        /// </summary>
        private IModel _channel;        

        /// <summary>
        /// Value indicating whether the current instance is opened. 1 means opened, 0 means closed
        /// </summary>
        private long _isOpened;

        /// <summary>
        /// Gets a value indicating whether the current <see cref="RabbitMqMessageReceiver" /> is currently opened;
        /// </summary>
        /// <value><c>true</c> if this instance is opened; otherwise, <c>false</c></value>
        public bool IsOpened => Interlocked.Read(ref _isOpened) == 1;        

        /// <summary>
        /// The channel basic properties
        /// </summary>
        private IBasicProperties _channelBasicProperties;

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
            if (channelSettings.PublishQueueTimeoutInSec > 0 || channelSettings.PublishQueueLimit > 0)
            {
                _useQueue = true;
                _msgQueue = new ConcurrentQueue<PublishQueueItem>();
                _queueLimit = channelSettings.PublishQueueLimit > 1 ? _channelSettings.PublishQueueLimit : -1;
                _queueTimeout = channelSettings.PublishQueueTimeoutInSec >= 10 ? _channelSettings.PublishQueueTimeoutInSec : 15;
                _queueTimer = new SdkTimer(new TimeSpan(0, 0, 5), new TimeSpan(0, 0, 1));
                _queueTimer.Elapsed += OnTimerElapsed;
                _queueTimer.FireOnce(new TimeSpan(0, 0, 5));
            }
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
                        if (pqi.Timestamp < DateTime.Now.AddSeconds(-_queueTimeout))
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
                            FeedLog.Info($"Publishing queue item succeeded. CorrelationId={pqi.CorrelationId}, RoutingKey={pqi.RoutingKey}, Added={pqi.Timestamp}.");
                        }
                        else
                        {
                            FeedLog.Warn($"Publishing queue item failed. CorrelationId={pqi.CorrelationId}, RoutingKey={pqi.RoutingKey}, Added={pqi.Timestamp}. Reason={publishResult.Message}");
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
                _queueTimer.FireOnce(TimeSpan.FromMilliseconds(300)); // recheck after X milliseconds
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
            if (_isOpened == 0)
            {
                throw new InvalidOperationException("The instance is closed");
            }
            try
            {
                if (_channelBasicProperties.Headers == null)
                {
                    _channelBasicProperties.Headers = new Dictionary<string, object>();
                }
                if (!string.IsNullOrEmpty(correlationId))
                {
                    //_channelBasicProperties.Headers["correlationId"] = correlationId;
                    _channelBasicProperties.CorrelationId = correlationId;
                }
                if (!string.IsNullOrEmpty(replyRoutingKey))
                {
                    _channelBasicProperties.Headers["replyRoutingKey"] = replyRoutingKey;
                }

                //FeedLog.Debug($"BasicPublish msg with correlationId:{correlationId}.");
                _channel.BasicPublish(_mtsChannelSettings.ExchangeName, routingKey, _channelBasicProperties, msg);
                return new MqPublishResult(correlationId);
            }
            catch (Exception e)
            {
                FeedLog.Info($"BasicPublish of message with correlationId:{correlationId} and routingKey:{routingKey} failed.", e);
                return new MqPublishResult(correlationId, false, e.Message);
            }
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
            if (_isOpened == 0)
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
            return await Task.Run(() => Publish(msg, routingKey, correlationId, replyRoutingKey));
        }

        /// <summary>
        /// Opens the current channel and binds the created queue to provided routing keys
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The instance is already opened</exception>
        public void Open()
        {
            if (Interlocked.CompareExchange(ref _isOpened, 1, 0) != 0)
            {
                ExecutionLog.Error("Opening an already opened channel is not allowed");
                throw new InvalidOperationException("The instance is already opened");
            }

            _channel = _channelFactory.CreateChannel();
            ExecutionLog.Info($"Opening the 'Publisher' channel with channelNumber: {_channel.ChannelNumber} and exchangeName: {_mtsChannelSettings.ExchangeName}.");

            // try to declare the exchange if it is not the default one
            if (!string.IsNullOrEmpty(_mtsChannelSettings.ExchangeName))
            {
                try
                {
                    _channel.ExchangeDeclare(_mtsChannelSettings.ExchangeName,
                                                _mtsChannelSettings.ExchangeType.ToString().ToLower(),
                                                _channelSettings.QueueIsDurable,
                                                false,
                                                null);
                }
                catch (Exception ie)
                {
                    ExecutionLog.Error(ie.Message, ie);
                    ExecutionLog.Warn($"Exchange {_mtsChannelSettings.ExchangeName} creation failed, will try to recreate it.");
                    _channel.ExchangeDelete(_mtsChannelSettings.ExchangeName);
                    _channel.ExchangeDeclare(_mtsChannelSettings.ExchangeName,
                                                _mtsChannelSettings.ExchangeType.ToString().ToLower(),
                                                _channelSettings.QueueIsDurable,
                                                false,
                                                null);
                }
            }

            _channelBasicProperties = _channel.CreateBasicProperties();
            _channelBasicProperties.ContentType = "application/json";
            _channelBasicProperties.DeliveryMode = _channelSettings.UsePersistentDeliveryMode ? (byte)2 : (byte)1;

            //headerProperties like replyRoutingKey
            _channelBasicProperties.Headers = new Dictionary<string, object>();
            if (_mtsChannelSettings.HeaderProperties != null && _mtsChannelSettings.HeaderProperties.Any())
            {
                foreach (var h in _mtsChannelSettings.HeaderProperties)
                {
                    _channelBasicProperties.Headers.Add(h.Key, h.Value);
                }
            }
        }

        /// <summary>
        /// Closes the current channel
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The instance is already closed</exception>
        public void Close()
        {
            Contract.Assume(_channel != null);
            if (Interlocked.CompareExchange(ref _isOpened, 0, 1) != 1)
            {
                ExecutionLog.Error($"Cannot close the channel on channelNumber: {_channel.ChannelNumber}, because this channel is already closed.");
                throw new InvalidOperationException("The instance is already closed");
            }

            _channel.Dispose();
        }
    }
}
