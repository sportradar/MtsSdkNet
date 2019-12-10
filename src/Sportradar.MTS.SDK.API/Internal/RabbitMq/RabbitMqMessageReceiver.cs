﻿/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Dawn;
using System.Linq;
using System.Text;
using log4net;
using Metrics;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sportradar.MTS.SDK.Common.Internal.Metrics;
using Sportradar.MTS.SDK.Common.Log;
using Sportradar.MTS.SDK.Entities.Enums;
using Sportradar.MTS.SDK.Entities.EventArguments;

namespace Sportradar.MTS.SDK.API.Internal.RabbitMq
{
    /// <summary>
    /// A <see cref="IRabbitMqMessageReceiver" /> implementation using RabbitMQ broker to deliver feed messages
    /// </summary>
    /// <seealso cref="IRabbitMqMessageReceiver" />
    public sealed class RabbitMqMessageReceiver : IRabbitMqMessageReceiver, IHealthStatusProvider
    {
        /// <summary>
        /// A log4net.ILog used for feed traffic logging
        /// </summary>
        private static readonly ILog FeedLog = SdkLoggerFactory.GetLoggerForFeedTraffic(typeof(RabbitMqMessageReceiver));

        /// <summary>
        /// A <see cref="IRabbitMqConsumerChannel" /> representing a channel to the RabbitMQ broker
        /// </summary>
        private readonly IRabbitMqConsumerChannel _consumerChannel;

        /// <summary>
        /// Gets a value indicating whether the current <see cref="RabbitMqMessageReceiver" /> is currently opened;
        /// </summary>
        /// <value><c>true</c> if this instance is opened; otherwise, <c>false</c></value>
        public bool IsOpened => _consumerChannel.IsOpened;

        /// <summary>
        /// Event raised when the <see cref="IRabbitMqConsumerChannel" /> receives the message
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs> MqMessageReceived;

        ///// <summary>
        ///// Event raised when the <see cref="IRabbitMqConsumerChannel" /> could not deserialize the received message
        ///// </summary>
        //public event EventHandler<MessageDeserializationFailedEventArgs> MqMessageDeserializationFailed;

        private readonly TicketResponseType _expectedTicketResponseType;

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqMessageReceiver" /> class
        /// </summary>
        /// <param name="channel">A <see cref="IRabbitMqConsumerChannel" /> representing a consumer channel to the RabbitMQ broker</param>
        /// <param name="expectedResponseType">The type of the message receiver is expecting</param>
        public RabbitMqMessageReceiver(IRabbitMqConsumerChannel channel, TicketResponseType expectedResponseType)
        {
            Guard.Argument(channel).NotNull();

            _consumerChannel = channel;
            _expectedTicketResponseType = expectedResponseType;
        }

        /// <summary>
        /// Handles the message received event
        /// </summary>
        /// <param name="sender">The <see cref="object" /> representation of the event sender</param>
        /// <param name="eventArgs">A <see cref="BasicDeliverEventArgs" /> containing event information</param>
        private void Consumer_OnMessageReceived(object sender, BasicDeliverEventArgs eventArgs)
        {
            if (eventArgs?.Body == null || !eventArgs.Body.Any())
            {
                FeedLog.WarnFormat("A message with {0} body received. Aborting message processing", eventArgs?.Body == null ? "null" : "empty");
                return;
            }

            var stopwatch = Stopwatch.StartNew();

            var messageBody = Encoding.UTF8.GetString(eventArgs.Body);
            var correlationId = string.Empty;
            var additionalInfo = new Dictionary<string, string>();
            if (eventArgs.BasicProperties != null)
            {
                correlationId = eventArgs.BasicProperties.CorrelationId;

                if (eventArgs.BasicProperties.IsHeadersPresent())
                {
                    object obj;
                    if (eventArgs.BasicProperties.Headers.ContainsKey("receivedUtcTimestamp"))
                    {
                        if (eventArgs.BasicProperties.Headers.TryGetValue("receivedUtcTimestamp", out obj))
                        {
                            //var date = TicketHelper.UnixTimeToDateTime((long)obj);
                            additionalInfo.Add("receivedUtcTimestamp", obj.ToString());
                        }
                    }
                    if (eventArgs.BasicProperties.Headers.ContainsKey("validatedUtcTimestamp"))
                    {
                        if (eventArgs.BasicProperties.Headers.TryGetValue("validatedUtcTimestamp", out obj))
                        {
                            //var date = TicketHelper.UnixTimeToDateTime((long)obj);
                            additionalInfo.Add("validatedUtcTimestamp", obj.ToString());
                        }
                    }
                    if (eventArgs.BasicProperties.Headers.ContainsKey("respondedUtcTimestamp"))
                    {
                        if (eventArgs.BasicProperties.Headers.TryGetValue("respondedUtcTimestamp", out obj))
                        {
                            //var date = TicketHelper.UnixTimeToDateTime((long)obj);
                            additionalInfo.Add("respondedUtcTimestamp", obj.ToString());
                        }
                    }
                    if (eventArgs.BasicProperties.Headers.ContainsKey("__uid__"))
                    {
                        if(eventArgs.BasicProperties.Headers.TryGetValue("__uid__", out obj))
                        {
                            var b = obj as byte[];
                            if (b != null)
                            {
                                var unused = Encoding.UTF8.GetString(b);
                            }
                        }
                    }
                    if (eventArgs.BasicProperties.Headers.ContainsKey("Content-Type"))
                    {
                        eventArgs.BasicProperties.Headers.TryGetValue("Content-Type", out obj);
                        var b = obj as byte[];
                        if (b != null)
                        {
                            var c = Encoding.UTF8.GetString(b);
                            additionalInfo.Add("Content-Type", c);
                        }
                    }
                }
            }
            if (FeedLog.IsDebugEnabled)
            {
                FeedLog.Debug($"CONSUME START Message: {correlationId}. Ticket: {messageBody}");
                if (eventArgs.BasicProperties != null)
                {
                    FeedLog.Debug($"CONSUME Message: {correlationId}. Properties: {WriteBasicProperties(eventArgs.BasicProperties)}");
                }
            }
            else
            {
                FeedLog.Info($"CONSUME START Message: {correlationId}.");
            }

            Metric.Context("RABBIT").Meter("RabbitMqMessageReceiver", Unit.Items).Mark(eventArgs.Exchange);

            RaiseMessageReceived(messageBody, eventArgs.RoutingKey, correlationId, additionalInfo.Any() ? additionalInfo : null);

            stopwatch.Stop();
            FeedLog.Info($"CONSUME END Message: {correlationId}. Processed in {stopwatch.ElapsedMilliseconds} ms.");
        }

        /// <summary>
        /// Raises the <see cref="MqMessageReceived" /> event
        /// </summary>
        /// <param name="body">The body of the message (json)</param>
        /// <param name="routingKey">The routing key</param>
        /// <param name="correlationId">The correlation id</param>
        /// <param name="additionalInfo">The additional information</param>
        private void RaiseMessageReceived(string body, string routingKey, string correlationId, IDictionary<string, string> additionalInfo)
        {
            Guard.Argument(body).NotNull().NotEmpty();

            MqMessageReceived?.Invoke(this, new MessageReceivedEventArgs(body, routingKey, correlationId, _expectedTicketResponseType, additionalInfo));
        }

        /// <summary>
        /// Opens the current instance
        /// </summary>
        public void Open()
        {
            _consumerChannel.ChannelMessageReceived += Consumer_OnMessageReceived;
            _consumerChannel.Open();
        }

        /// <summary>
        /// Opens the current <see cref="RabbitMqMessageReceiver" /> instance so it starts receiving messages
        /// </summary>
        /// <param name="routingKeys">A list of routing keys specifying which messages should the <see cref="RabbitMqMessageReceiver" /> deliver</param>
        public void Open(IEnumerable<string> routingKeys)
        {
            _consumerChannel.ChannelMessageReceived += Consumer_OnMessageReceived;
            _consumerChannel.Open(routingKeys);
        }

        /// <summary>
        /// Opens the current channel and binds the created queue to provided routing keys
        /// </summary>
        /// <param name="queueName">Name of the queue</param>
        /// <param name="routingKeys">A <see cref="IEnumerable{String}" /> specifying the routing keys of the constructed queue</param>
        public void Open(string queueName, IEnumerable<string> routingKeys)
        {
            _consumerChannel.ChannelMessageReceived += Consumer_OnMessageReceived;
            _consumerChannel.Open(queueName, routingKeys);
        }

        /// <summary>
        /// Closes the current <see cref="RabbitMqMessageReceiver" /> so it will no longer receive messages
        /// </summary>
        public void Close()
        {
            _consumerChannel.ChannelMessageReceived -= Consumer_OnMessageReceived;
            _consumerChannel.Close();
        }

        /// <summary>
        /// Registers the health check which will be periodically triggered
        /// </summary>
        public void RegisterHealthCheck()
        {
            HealthChecks.RegisterHealthCheck("RabbitMqMessageReceiver", StartHealthCheck);
        }

        /// <summary>
        /// Starts the health check and returns <see cref="HealthCheckResult" />
        /// </summary>
        /// <returns>HealthCheckResult</returns>
        public HealthCheckResult StartHealthCheck()
        {
            return HealthCheckResult.Healthy("RabbitMqMessageReceiver is operational.");
        }

        private static string WriteBasicProperties(IBasicProperties props)
        {
            if (props == null)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            //{:content_type=>"application/json", :headers=> {"validatedUtcTimestamp"=>1542078341898, "receivedUtcTimestamp"=>1542078341892, "respondedUtcTimestamp"=>1542078341908, "_uid_"=>"b6a6e220-2cd3-4d8e-99b9-684c1bc66a13", "Content-Type"=>"application/json"}, :delivery_mode => 1, :priority => 0, :correlation_id => "j9501f21e-6264-44b1-a83d-6689303c4e31"}
            sb.Append("ContentType=").Append(props.ContentType);
            if (props.IsHeadersPresent())
            {
                sb.Append(", Headers={").Append(string.Join(", ", props.Headers.Select(s=> $"{s.Key}={WriteHeaderValue(s.Value)}"))).Append("}");
            }
            sb.Append(", DeliveryMode=").Append(props.DeliveryMode);
            sb.Append(", Priority=").Append(props.Priority);
            sb.Append(", CorrelationId=").Append(props.CorrelationId);
            if (props.IsTimestampPresent())
            {
                sb.Append(", Timestamp=").Append(props.Timestamp.UnixTime);
            }

            return sb.ToString();
        }

        private static string WriteHeaderValue(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if (value is byte[] bytes)
            {
                return Encoding.UTF8.GetString(bytes);
            }
            return value.ToString();
        }
    }
}
