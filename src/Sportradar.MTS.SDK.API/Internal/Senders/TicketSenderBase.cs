/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using log4net;
using Sportradar.MTS.SDK.API.Internal.RabbitMq;
using Sportradar.MTS.SDK.Common.Internal;
using Sportradar.MTS.SDK.Common.Log;
using Sportradar.MTS.SDK.Entities.EventArguments;
using Sportradar.MTS.SDK.Entities.Interfaces;
using Sportradar.MTS.SDK.Entities.Internal;

namespace Sportradar.MTS.SDK.API.Internal.Senders
{
    /// <summary>
    /// Base implementation of the <see cref="ITicketSender"/>
    /// </summary>
    /// <seealso cref="ITicketSender" />
    public abstract class TicketSenderBase : ITicketSender
    {
        /// <summary>
        /// The log
        /// </summary>
        private readonly ILog _executionLog = SdkLoggerFactory.GetLogger(typeof(TicketSenderBase));
        private readonly ILog _feedLog = SdkLoggerFactory.GetLoggerForFeedTraffic(typeof(TicketSenderBase));

        /// <summary>
        /// Raised when the attempt to send ticket failed
        /// </summary>
        public event EventHandler<TicketSendFailedEventArgs> TicketSendFailed;

        /// <summary>
        /// The publisher channel
        /// </summary>
        private readonly IRabbitMqPublisherChannel _publisherChannel;
        /// <summary>
        /// The ticket cache
        /// </summary>
        private readonly ConcurrentDictionary<string, TicketCacheItem> _ticketCache;
        /// <summary>
        /// The MTS channel settings
        /// </summary>
        private readonly IMtsChannelSettings _mtsChannelSettings;

        /// <summary>
        /// The timer
        /// </summary>
        private readonly ITimer _timer;

        private bool _isOpened;

        /// <summary>
        /// Gets the get cache timeout
        /// </summary>
        /// <value>The get cache timeout</value>
        public int GetCacheTimeout { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketSenderBase"/> class
        /// </summary>
        /// <param name="publisherChannel">The publisher channel</param>
        /// <param name="ticketCache">The ticket cache</param>
        /// <param name="mtsChannelSettings">The MTS channel settings</param>
        /// <param name="ticketCacheTimeoutInSec">The ticket cache timeout in sec</param>
        internal TicketSenderBase(IRabbitMqPublisherChannel publisherChannel,
                              ConcurrentDictionary<string, TicketCacheItem> ticketCache,
                              IMtsChannelSettings mtsChannelSettings,
                              int ticketCacheTimeoutInSec)
        {
            Contract.Requires(publisherChannel != null);
            Contract.Requires(ticketCache != null);
            Contract.Requires(mtsChannelSettings != null);

            _publisherChannel = publisherChannel;
            _ticketCache = ticketCache;
            _mtsChannelSettings = mtsChannelSettings;

            _publisherChannel.MqMessagePublishFailed += PublisherChannelOnMqMessagePublishFailed;

            GetCacheTimeout = ticketCacheTimeoutInSec < 10 ? 20 : ticketCacheTimeoutInSec;

            _timer = new SdkTimer(new TimeSpan(0, 0, GetCacheTimeout), new TimeSpan(0, 0, 10));
            _timer.Elapsed += OnTimerElapsed;
            _timer.FireOnce(new TimeSpan(0, 0, GetCacheTimeout));
        }

        private void PublisherChannelOnMqMessagePublishFailed(object sender,
            MessagePublishFailedEventArgs messagePublishFailedEventArgs)
        {
            _executionLog.Info($"Message publishing failed with correlationId: {messagePublishFailedEventArgs.CorrelationId}, errorMessage: {messagePublishFailedEventArgs.ErrorMessage}, routingKey: {messagePublishFailedEventArgs.RoutingKey}.");

            var ticketId = string.Empty;
            var ci = _ticketCache.Values.FirstOrDefault(f => f.CorrelationId == messagePublishFailedEventArgs.CorrelationId);
            if (!string.IsNullOrEmpty(ci?.TicketId))
            {
                ticketId = ci.TicketId;
            }
            var json = Encoding.UTF8.GetString(messagePublishFailedEventArgs.RawData.ToArray());

            var arg = new TicketSendFailedEventArgs(ticketId, json, messagePublishFailedEventArgs.ErrorMessage);
            TicketSendFailed?.Invoke(sender, arg);
        }

        /// <summary>
        /// Defines invariant members of the class
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_publisherChannel != null);
            Contract.Invariant(_ticketCache != null);
            Contract.Invariant(_mtsChannelSettings != null);
            Contract.Invariant(_feedLog != null);
            Contract.Invariant(_executionLog != null);
        }

        /// <summary>
        /// Handles the <see cref="E:TimerElapsed" /> event
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data</param>
        private void OnTimerElapsed(object sender, EventArgs e)
        {
            try
            {
                DeleteExpiredCacheItems();
            }
            catch (Exception exception)
            {
                _executionLog.Warn("Error during cleaning TicketCache.", exception);
            }
            _timer.FireOnce(new TimeSpan(0, 0, 10));
        }

        /// <summary>
        /// Deletes the expired cache items
        /// </summary>
        private void DeleteExpiredCacheItems()
        {
            var expiredItems = _ticketCache.Where(t => t.Value.Timestamp < DateTime.Now.AddSeconds(-GetCacheTimeout));
            foreach (var ei in expiredItems)
            {
                TicketCacheItem ci;
                _ticketCache.TryRemove(ei.Key, out ci);
            }
        }

        /// <summary>
        /// Gets the mapped dto json MSG
        /// </summary>
        /// <param name="sdkTicket">The SDK ticket</param>
        /// <returns>System.String</returns>
        protected abstract string GetMappedDtoJsonMsg(ISdkTicket sdkTicket);

        /// <summary>
        /// Gets the byte MSG
        /// </summary>
        /// <param name="sdkTicket">The SDK ticket</param>
        /// <returns>System.Byte[]</returns>
        protected byte[] GetByteMsg(ISdkTicket sdkTicket)
        {
            var json = GetMappedDtoJsonMsg(sdkTicket);
            if (_feedLog.IsDebugEnabled)
            {
                _feedLog.Debug($"Sending {sdkTicket.GetType().Name}: {json}");
            }
            else
            {
                _feedLog.Info($"Sending {sdkTicket.GetType().Name} with ticketId: {sdkTicket.TicketId}.");
            }
            if (_executionLog.IsDebugEnabled)
            {
                _executionLog.Debug($"Sending {sdkTicket.GetType().Name}: {json}");
            }
            else
            {
                _executionLog.Info($"Sending {sdkTicket.GetType().Name} with ticketId: {sdkTicket.TicketId}.");
            }
            var msg = Encoding.UTF8.GetBytes(json);
            return msg;
        }

        /// <summary>
        /// Sends the ticket
        /// </summary>
        /// <param name="sdkTicket">The <see cref="ISdkTicket"/> to be send</param>
        public void SendTicket(ISdkTicket sdkTicket)
        {
            var msg = GetByteMsg(sdkTicket);
            if (string.IsNullOrEmpty(sdkTicket.CorrelationId))
            {
                _feedLog.Warn($"Ticket: {sdkTicket.TicketId} is missing CorrelationId.");
            }

            var ticketCI = new TicketCacheItem(TicketHelper.GetTicketTypeFromTicket(sdkTicket), sdkTicket.TicketId, sdkTicket.CorrelationId, _mtsChannelSettings.ReplyToRoutingKey, null, sdkTicket);

            // we clear cache, since already sent ticket with the same ticketId are obsolete (example: sending ticket, ticketAck, ticketCancel, ticketCancelAck)
            TicketCacheItem oldTicket;
            if (_ticketCache.TryRemove(sdkTicket.TicketId, out oldTicket))
            {
                _executionLog.Debug($"Removed already sent ticket from cache {sdkTicket.TicketId}");
            }

            _ticketCache.TryAdd(sdkTicket.TicketId, ticketCI);
            _publisherChannel.Publish(msg: msg, routingKey: _mtsChannelSettings.PublishRoutingKey, correlationId: sdkTicket.CorrelationId, replyRoutingKey: _mtsChannelSettings.ReplyToRoutingKey);
        }

        /// <summary>
        /// Gets the sent ticket
        /// </summary>
        /// <param name="ticketId">The ticket identifier</param>
        /// <returns>ISdkTicket</returns>
        public ISdkTicket GetSentTicket(string ticketId)
        {
            TicketCacheItem ci;
            if (_ticketCache.TryRemove(ticketId, out ci))
            {
                return TicketHelper.GetTicketInSpecificType(ci);
            }
            return null;
        }

        /// <summary>
        /// Gets a value indicating whether the current instance is opened
        /// </summary>
        /// <value><c>true</c> if this instance is opened; otherwise, <c>false</c></value>
        public bool IsOpened => _isOpened;

        /// <summary>
        /// Opens the current instance
        /// </summary>
        public void Open()
        {
            _publisherChannel.Open();
            _isOpened = true;
        }

        /// <summary>
        /// Closes the current instance
        /// </summary>
        public void Close()
        {
            _publisherChannel.Close();
            _isOpened = false;
        }
    }
}
