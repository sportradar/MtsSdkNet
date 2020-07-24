﻿/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections.Generic;
using Dawn;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using log4net;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using Sportradar.MTS.SDK.Common.Internal;
using Sportradar.MTS.SDK.Common.Log;

namespace Sportradar.MTS.SDK.API.Internal.RabbitMq
{
    /// <summary>
    /// A class used to connect to the Rabbit MQ broker
    /// </summary>
    /// <seealso cref="IRabbitMqConsumerChannel" />
    public class RabbitMqConsumerChannel : IRabbitMqConsumerChannel
    {
        /// <summary>
        /// Gets the unique identifier
        /// </summary>
        /// <value>The unique identifier</value>
        public int UniqueId { get; }

        /// <summary>
        /// The log4net.ILog used execution logging
        /// </summary>
        private static readonly ILog ExecutionLog = SdkLoggerFactory.GetLogger(typeof(RabbitMqConsumerChannel));

        /// <summary>
        /// The feed log
        /// </summary>
        private static readonly ILog FeedLog = SdkLoggerFactory.GetLoggerForFeedTraffic(typeof(RabbitMqConsumerChannel));

        /// <summary>
        /// A <see cref="IChannelFactory"/> used to construct the <see cref="IModel"/> representing Rabbit MQ channel
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
        /// Gets a value indicating whether the current <see cref="RabbitMqMessageReceiver"/> is currently opened;
        /// </summary>
        public bool IsOpened => Interlocked.Read(ref _isOpened) == 1;

        /// <summary>
        /// Occurs when the current channel received the data
        /// </summary>
        public event EventHandler<BasicDeliverEventArgs> ChannelMessageReceived;

        private readonly IMtsChannelSettings _mtsChannelSettings;

        private readonly IRabbitMqChannelSettings _channelSettings;

        private string _queueName;

        private IEnumerable<string> _routingKeys;

        /// <summary>
        /// The queue timer
        /// </summary>
        private readonly ITimer _healthTimer;

        private readonly int _timerInterval = 180;

        private readonly object _lock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqConsumerChannel"/> class
        /// </summary>
        /// <param name="channelFactory">A <see cref="IChannelFactory"/> used to construct the <see cref="IModel"/> representing Rabbit MQ channel</param>
        /// <param name="mtsChannelSettings"></param>
        /// <param name="channelSettings"></param>
        public RabbitMqConsumerChannel(IChannelFactory channelFactory,
                                       IMtsChannelSettings mtsChannelSettings,
                                       IRabbitMqChannelSettings channelSettings)
        {
            Guard.Argument(channelFactory, nameof(channelFactory)).NotNull();
            Guard.Argument(mtsChannelSettings, nameof(mtsChannelSettings)).NotNull();

            _channelFactory = channelFactory;
            _mtsChannelSettings = mtsChannelSettings;
            _channelSettings = channelSettings;

            _queueName = _mtsChannelSettings.ChannelQueueName;

            _shouldBeOpened = 0;

            UniqueId = _channelFactory.GetUniqueId();
            _healthTimer = new SdkTimer(new TimeSpan(0, 0, _timerInterval), new TimeSpan(0, 0, 1));
            _healthTimer.Elapsed += OnTimerElapsed;
        }

        /// <summary>
        /// Invoked when the internally used timer elapses
        /// </summary>
        /// <param name="sender">A <see cref="object" /> representation of the <see cref="ITimer" /> raising the event</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data</param>
        private void OnTimerElapsed(object sender, EventArgs e)
        {
            try
            {
                CreateAndOpenConsumerChannel();
            }
            catch (Exception)
            {
                // ignored
            }

            if (_shouldBeOpened == 1)
            {
                _healthTimer.FireOnce(TimeSpan.FromSeconds(_timerInterval));
            }
        }

        /// <summary>
        /// Handles the <see cref="EventingBasicConsumer.Received"/> event
        /// </summary>
        /// <param name="sender">The <see cref="object"/> representation of the instance raising the event</param>
        /// <param name="basicDeliverEventArgs">The <see cref="BasicDeliverEventArgs"/> instance containing the event data</param>
        private void OnDataReceived(object sender, BasicDeliverEventArgs basicDeliverEventArgs)
        {
            var correlationId = basicDeliverEventArgs?.BasicProperties?.CorrelationId ?? string.Empty;
            FeedLog.Info($"Received message from MTS with correlationId: {correlationId}.");
            ChannelMessageReceived?.Invoke(this, basicDeliverEventArgs);

            // TODO: ??? should this be in any way depended on user action (only acknowledged that message was received)
            if (_channelSettings.UserAcknowledgmentEnabled)
            {
                var i = 0;
                while (i < 10)
                {
                    i++;
                    try
                    {
                        var channelWrapper = _channelFactory.GetChannel(UniqueId);
                        CreateAndOpenConsumerChannel();
                        channelWrapper.Channel.BasicAck(basicDeliverEventArgs?.DeliveryTag ?? 0, false);
                        break;
                    }
                    catch (Exception e)
                    {
                        if (!e.Message.Contains("unknown delivery tag"))
                        {
                            FeedLog.Debug($"Sending Ack for processed message {basicDeliverEventArgs?.DeliveryTag} failed. {e.Message}");
                        }
                        Thread.Sleep(i * 1000);
                    }
                }
            }
        }

        private void OnConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            ExecutionLog.Info($"Cancelled consumer channel with channelNumber: {UniqueId} and queueName: {_queueName}.");
        }

        private void OnRegistered(object sender, ConsumerEventArgs e)
        {
            ExecutionLog.Info($"Registered consumer channel with channelNumber: {UniqueId} and queueName: {_queueName}.");
        }

        private void OnUnregistered(object sender, ConsumerEventArgs e)
        {
            ExecutionLog.Info($"Unregistered consumer channel with channelNumber: {UniqueId} and queueName: {_queueName}.");
        }

        private void OnShutdown(object sender, ShutdownEventArgs e)
        {
            ExecutionLog.Info($"Shutdown consumer channel with channelNumber: {UniqueId} and queueName: {_queueName}. Reason={e.ReplyCode}-{e.ReplyText}, Cause={e.Cause}");
            Interlocked.CompareExchange(ref _isOpened, 0, 1);
            _healthTimer.FireOnce(TimeSpan.FromMilliseconds(100));
        }

        /// <summary>
        /// Opens the current channel and binds the created queue to provided routing keys
        /// </summary>
        public void Open()
        {
            Open(_mtsChannelSettings.ChannelQueueName, _mtsChannelSettings.RoutingKeys);
        }

        public void Open(IEnumerable<string> routingKeys)
        {
            Guard.Argument(routingKeys, nameof(routingKeys)).NotNull();//.NotEmpty();
            if (!routingKeys.Any())
                throw new ArgumentOutOfRangeException(nameof(routingKeys));

            Open(_mtsChannelSettings.ChannelQueueName, routingKeys);
        }

        public void Open(string queueName, IEnumerable<string> routingKeys)
        {
            Guard.Argument(queueName, nameof(queueName)).NotNull().NotEmpty();
            Guard.Argument(routingKeys, nameof(routingKeys)).NotNull();//.NotEmpty();
            if (!routingKeys.Any())
                throw new ArgumentOutOfRangeException(nameof(routingKeys));

            if (Interlocked.Read(ref _isOpened) == 1)
            {
                ExecutionLog.Error("Opening an already opened channel is not allowed");
                throw new InvalidOperationException("The instance is already opened");
            }

            if (!string.IsNullOrEmpty(queueName))
            {
                _queueName = queueName;
            }

            _routingKeys = routingKeys;

            //Interlocked.CompareExchange(ref _isOpened, 1, 0);
            Interlocked.CompareExchange(ref _shouldBeOpened, 1, 0);
            CreateAndOpenConsumerChannel();
            _healthTimer.FireOnce(new TimeSpan(0, 0, _timerInterval));
        }

        private void CreateAndOpenConsumerChannel()
        {
            var sleepTime = 1000;
            while (Interlocked.Read(ref _shouldBeOpened) == 1)
            {
                lock (_lock)
                {
                    try
                    {

                        //ExecutionLog.Debug($"Opening the consumer channel with channelNumber: {UniqueId} and queueName: {_queueName} started ...");
                        var channelWrapper = _channelFactory.GetChannel(UniqueId);
                        if (channelWrapper == null)
                        {
                            throw new OperationCanceledException("Missing consumer channel wrapper.");
                        }

                        if (channelWrapper.MarkedForDeletion)
                        {
                            _channelFactory.RemoveChannel(UniqueId);
                            throw new OperationCanceledException("Consumer channel marked for deletion.");
                        }

                        if (channelWrapper.Channel.IsClosed && !channelWrapper.MarkedForDeletion)
                        {
                            channelWrapper.MarkedForDeletion = true;
                            DisposeCurrentConsumer(channelWrapper);
                            _channelFactory.RemoveChannel(UniqueId);
                            continue;
                        }

                        if (channelWrapper.Channel.IsOpen && channelWrapper.Consumer != null)
                        {
                            return;
                        }

                        ExecutionLog.Info($"Opening the consumer channel with channelNumber: {UniqueId} and queueName: {_queueName}.");

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

                        var arguments = new Dictionary<string, object> { { "x-queue-master-locator", "min-masters" } };

                        var declareResult = _channelSettings.QueueIsDurable
                            ? channelWrapper.Channel.QueueDeclare(_queueName, true, false, false, arguments)
                            : channelWrapper.Channel.QueueDeclare(_queueName, false, false, false, arguments);

                        if (!string.IsNullOrEmpty(_mtsChannelSettings.ExchangeName) && _routingKeys != null)
                        {
                            foreach (var routingKey in _routingKeys)
                            {
                                ExecutionLog.Info($"Binding queue={declareResult.QueueName} with routingKey={routingKey}");
                                channelWrapper.Channel.QueueBind(declareResult.QueueName,
                                    exchange: _mtsChannelSettings.ExchangeName,
                                    routingKey: routingKey);
                            }
                        }

                        channelWrapper.Channel.BasicQos(0, 10, false);
                        var consumer = new EventingBasicConsumer(channelWrapper.Channel);
                        consumer.Received += OnDataReceived;
                        consumer.Registered += OnRegistered;
                        consumer.Unregistered += OnUnregistered;
                        consumer.Shutdown += OnShutdown;
                        consumer.ConsumerCancelled += OnConsumerCancelled;
                        channelWrapper.Channel.BasicConsume(queue: declareResult.QueueName,
                            noAck: !_channelSettings.UserAcknowledgmentEnabled,
                            consumerTag: $"{_mtsChannelSettings.ConsumerTag}",
                            consumer: consumer,
                            noLocal: false,
                            exclusive: _channelSettings.ExclusiveConsumer);
                        channelWrapper.Consumer = consumer;

                        Interlocked.CompareExchange(ref _isOpened, 1, 0);
                        //ExecutionLog.Debug($"Opening the consumer channel with channelNumber: {UniqueId} and queueName: {_queueName} finished.");
                        return;
                    }
                    catch (Exception e)
                    {
                        ExecutionLog.Info($"Error opening the consumer channel with channelNumber: {UniqueId} and queueName: {_queueName}.",
                            e);
                        if (e is IOException || e is AlreadyClosedException || e is SocketException)
                        {
                            sleepTime = SdkInfo.Increase(sleepTime, 500, 10000);
                        }
                        else
                        {
                            sleepTime = SdkInfo.Multiply(sleepTime, 1.25, _channelSettings.PublishQueueTimeoutInMs1 * 1000);
                        }

                        ExecutionLog.Info($"Opening the consumer channel will be retried in next {sleepTime} ms.");
                        Thread.Sleep(sleepTime);
                    }
                }
            }
        }

        /// <summary>
        /// Closes the current channel
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The instance is already closed</exception>
        public void Close()
        {
            if (Interlocked.CompareExchange(ref _isOpened, 0, 1) != 1)
            {
                ExecutionLog.Error($"Cannot close the consumer channel on channelNumber: {UniqueId}, because this channel is already closed.");
                //throw new InvalidOperationException("The instance is already closed");
            }
            Interlocked.CompareExchange(ref _shouldBeOpened, 0, 1);
        }

        private void DisposeCurrentConsumer(ChannelWrapper channelWrapper)
        {
            if (channelWrapper?.Consumer != null)
            {
                ExecutionLog.Info($"Closing the consumer channel with channelNumber: {UniqueId} and queueName: {_queueName}.");
                channelWrapper.Consumer.Received -= OnDataReceived;
                channelWrapper.Consumer.Registered -= OnRegistered;
                channelWrapper.Consumer.Unregistered -= OnUnregistered;
                channelWrapper.Consumer.Shutdown -= OnShutdown;
                channelWrapper.Consumer.ConsumerCancelled -= OnConsumerCancelled;
                channelWrapper.Consumer = null;
            }
        }
    }
}
