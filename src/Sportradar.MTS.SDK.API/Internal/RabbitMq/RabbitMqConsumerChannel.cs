/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using log4net;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using Sportradar.MTS.SDK.Common.Exceptions;
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
        /// The log4net.ILog used execution logging
        /// </summary>
        private static readonly ILog ExecutionLog = SdkLoggerFactory.GetLogger(typeof(RabbitMqConsumerChannel));
        private static readonly ILog FeedLog = SdkLoggerFactory.GetLoggerForFeedTraffic(typeof(RabbitMqConsumerChannel));

        /// <summary>
        /// A <see cref="IChannelFactory"/> used to construct the <see cref="IModel"/> representing Rabbit MQ channel
        /// </summary>
        private readonly IChannelFactory _channelFactory;

        /// <summary>
        /// The <see cref="IModel"/> representing the channel to the broker
        /// </summary>
        private IModel _channel;

        /// <summary>
        /// The <see cref="EventingBasicConsumer"/> used to consume the broker data
        /// </summary>
        private EventingBasicConsumer _consumer;

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
        /// Initializes a new instance of the <see cref="RabbitMqConsumerChannel"/> class
        /// </summary>
        /// <param name="channelFactory">A <see cref="IChannelFactory"/> used to construct the <see cref="IModel"/> representing Rabbit MQ channel</param>
        /// <param name="mtsChannelSettings"></param>
        /// <param name="channelSettings"></param>
        public RabbitMqConsumerChannel(IChannelFactory channelFactory,
                                       IMtsChannelSettings mtsChannelSettings,
                                       IRabbitMqChannelSettings channelSettings)
        {
            Contract.Requires(channelFactory != null);
            Contract.Requires(mtsChannelSettings != null);

            _channelFactory = channelFactory;

            _mtsChannelSettings = mtsChannelSettings;
            _channelSettings = channelSettings;

            _queueName = _mtsChannelSettings.ChannelQueueName;

            _shouldBeOpened = 0;
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
        /// Handles the <see cref="EventingBasicConsumer.Received"/> event
        /// </summary>
        /// <param name="sender">The <see cref="object"/> representation of the instance raising the event</param>
        /// <param name="basicDeliverEventArgs">The <see cref="BasicDeliverEventArgs"/> instance containing the event data</param>
        private void OnDataReceived(object sender, BasicDeliverEventArgs basicDeliverEventArgs)
        {
            var correlationId = basicDeliverEventArgs?.BasicProperties?.CorrelationId ?? string.Empty;
            FeedLog.Info($"Received response from MTS with correlationId: {correlationId}.");
            ChannelMessageReceived?.Invoke(this, basicDeliverEventArgs);

            // TODO: ??? should this be in any way depended on user action (only acknowledged that message was received)
            if (_channelSettings.UserAcknowledgmentEnabled)
            {
                _channel.BasicAck(basicDeliverEventArgs?.DeliveryTag ?? 0, false);
            }
        }

        private void OnConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            ExecutionLog.Debug($"Cancelled consumer channel with channelNumber: {_channel?.ChannelNumber} and queueName: {_queueName}.");
        }

        private void OnRegistered(object sender, ConsumerEventArgs e)
        {
            ExecutionLog.Debug($"Registered consumer channel with channelNumber: {_channel?.ChannelNumber} and queueName: {_queueName}.");
        }

        private void OnUnregistered(object sender, ConsumerEventArgs e)
        {
            ExecutionLog.Debug($"Unregistered consumer channel with channelNumber: {_channel?.ChannelNumber} and queueName: {_queueName}.");
        }

        private void OnShutdown(object sender, ShutdownEventArgs e)
        {
            ExecutionLog.Info($"ShutDown consumer channel with channelNumber: {_channel?.ChannelNumber} and queueName: {_queueName}. Reason={e.ReplyCode}-{e.ReplyText}");
            Interlocked.CompareExchange(ref _isOpened, 0, 1);
            //320-connection closed by the management console
            if (e.ReplyCode == 320 || e.ReplyCode > 0)
            {
                CreateAndOpenConsumerChannel();
            }
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
            Open(_mtsChannelSettings.ChannelQueueName, routingKeys);
        }

        public void Open(string queueName, IEnumerable<string> routingKeys)
        {
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

            Interlocked.CompareExchange(ref _shouldBeOpened, 1, 0);
            CreateAndOpenConsumerChannel();
        }

        /// <summary>
        /// Closes the current channel
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The instance is already closed</exception>
        public void Close()
        {
            if (Interlocked.CompareExchange(ref _isOpened, 0, 1) != 1)
            {
                ExecutionLog.Error($"Cannot close the channel on channelNumber: {_channel.ChannelNumber}, because this channel is already closed.");
                throw new InvalidOperationException("The instance is already closed");
            }
            Interlocked.CompareExchange(ref _shouldBeOpened, 0, 1);

            DisposeCurrentChannel();
        }

        private void DisposeCurrentChannel()
        {
            if (_channel == null || _consumer == null)
            {
                return;
            }

            Contract.Assume(_channel != null);
            Contract.Assume(_consumer != null);
            ExecutionLog.Info($"Closing the channel with channelNumber: {_channel.ChannelNumber} and queueName: {_queueName}.");
            _consumer.Received -= OnDataReceived;
            _consumer.Registered -= OnRegistered;
            _consumer.Unregistered -= OnUnregistered;
            _consumer.Shutdown -= OnShutdown;
            _consumer.ConsumerCancelled -= OnConsumerCancelled;
            _consumer = null;

            if (_channelSettings.DeleteQueueOnClose)
            {
                var deleted = DeleteQueue();
                if (!deleted)
                {
                    ExecutionLog.Info($"Deleting of queue '{_queueName} failed. Channel will be disposed.");
                }
            }
            _channel.Dispose();
        }

        private bool DeleteQueue()
        {
            if (IsOpened)
            {
                throw new RabbitMqException("To delete queue, you need to close consumer first.", null);
            }

            try
            {
                const bool ifUnused = true;
                const bool ifEmpty = false;
                _channel.QueueDelete(_queueName, ifUnused, ifEmpty);
                return true;
            }
            catch (Exception)
            {
                //ExecutionLog.Debug($"Unable to delete queue '{_queueName}'.", e);
                ExecutionLog.Debug($"Unable to delete queue '{_queueName}'.");
            }
            return false;
        }

        private void CreateAndOpenConsumerChannel()
        {
            var sleepTime = 0;
            while (Interlocked.Read(ref _shouldBeOpened) == 1 && !IsOpened)
            {
                try
                {
                    var channel = _channelFactory.CreateChannel();
                    ExecutionLog.Info($"Opening the 'Consumer' channel with channelNumber: {channel.ChannelNumber} and queueName: {_queueName}.");

                    // try to declare the exchange if it is not the default one
                    if (!string.IsNullOrEmpty(_mtsChannelSettings.ExchangeName))
                    {
                        try
                        {
                            channel.ExchangeDeclare(_mtsChannelSettings.ExchangeName,
                                                    _mtsChannelSettings.ExchangeType.ToString().ToLower(),
                                                    _channelSettings.QueueIsDurable,
                                                    false,
                                                    null);
                        }
                        catch (Exception ie)
                        {
                            ExecutionLog.Error(ie.Message, ie);
                            ExecutionLog.Warn($"Exchange {_mtsChannelSettings.ExchangeName} creation failed, will try to recreate it.");
                            channel.ExchangeDelete(_mtsChannelSettings.ExchangeName);
                            channel.ExchangeDeclare(_mtsChannelSettings.ExchangeName,
                                                    _mtsChannelSettings.ExchangeType.ToString().ToLower(),
                                                    _channelSettings.QueueIsDurable,
                                                    false,
                                                    null);
                        }
                    }

                    var declareResult = _channelSettings.QueueIsDurable
                        ? channel.QueueDeclare(_queueName, true, false, false, null)
                        : channel.QueueDeclare(_queueName, false, false, false, null);

                    if (!string.IsNullOrEmpty(_mtsChannelSettings.ExchangeName) && _routingKeys != null)
                    {
                        foreach (var routingKey in _routingKeys)
                        {
                            ExecutionLog.Info($"Binding queue={declareResult.QueueName} with routingKey={routingKey}");
                            channel.QueueBind(declareResult.QueueName, exchange: _mtsChannelSettings.ExchangeName, routingKey: routingKey);
                        }
                    }
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += OnDataReceived;
                    consumer.Registered += OnRegistered;
                    consumer.Unregistered += OnUnregistered;
                    consumer.Shutdown += OnShutdown;
                    consumer.ConsumerCancelled += OnConsumerCancelled;
                    channel.BasicConsume(queue: declareResult.QueueName,
                                         noAck: !_channelSettings.UserAcknowledgmentEnabled,
                                         consumerTag:$"{_mtsChannelSettings.ConsumerTag}",
                                         consumer: consumer,
                                         noLocal: false,
                                         exclusive: _channelSettings.ExclusiveConsumer);

                    DisposeCurrentChannel();
                    _channel = channel;
                    _consumer = consumer;

                    Interlocked.CompareExchange(ref _isOpened, 1, 0);
                }
                catch (Exception e)
                {
                    ExecutionLog.Info($"Error opening the 'Consumer' channel with channelNumber: {_channel?.ChannelNumber} and queueName: {_queueName}.", e);
                    if (e is IOException || e is AlreadyClosedException || e is SocketException)
                    {
                        sleepTime = GetSleepTime(true, sleepTime);
                    }
                    else
                    {
                        sleepTime = GetSleepTime(false, sleepTime);
                    }

                    Thread.Sleep(sleepTime);
                }
            }
        }

        private int GetSleepTime(bool isConnectionException, int currentSleepTime)
        {
            if (currentSleepTime == 0 || !isConnectionException)
            {
                return 1000;
            }

            if (currentSleepTime < 64000)
            {
                currentSleepTime *= 2;
            }
            return currentSleepTime;
        }
    }
}
