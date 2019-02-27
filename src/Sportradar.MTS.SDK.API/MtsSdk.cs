/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using log4net;
using Metrics;
using Microsoft.Practices.Unity;
using RabbitMQ.Client.Exceptions;
using Sportradar.MTS.SDK.API.Internal;
using Sportradar.MTS.SDK.API.Internal.RabbitMq;
using Sportradar.MTS.SDK.API.Internal.Senders;
using Sportradar.MTS.SDK.Common.Exceptions;
using Sportradar.MTS.SDK.Common.Internal;
using Sportradar.MTS.SDK.Common.Log;
using Sportradar.MTS.SDK.Entities;
using Sportradar.MTS.SDK.Entities.Builders;
using Sportradar.MTS.SDK.Entities.EventArguments;
using Sportradar.MTS.SDK.Entities.Interfaces;
using Sportradar.MTS.SDK.Entities.Internal;

namespace Sportradar.MTS.SDK.API
{
    /// <summary>
    /// A <see cref="IMtsSdk"/> implementation acting as an entry point to the MTS SDK
    /// </summary>
    public class MtsSdk : IMtsSdk
    {
        /// <summary>
        /// A log4net.ILog instance used for logging execution logs
        /// </summary>
        private static readonly ILog ExecutionLog = SdkLoggerFactory.GetLogger(typeof(MtsSdk));

        /// <summary>
        /// A log4net.ILog instance used for logging client iteration logs
        /// </summary>
        private static readonly ILog InteractionLog = SdkLoggerFactory.GetLoggerForClientInteraction(typeof(MtsSdk));

        /// <summary>
        /// A <see cref="ConnectionValidator"/> used to detect potential connectivity issues
        /// </summary>
        private readonly ConnectionValidator _connectionValidator;

        private readonly EntitiesMapper _entitiesMapper;
        private readonly ITicketSenderFactory _ticketPublisherFactory;

        private readonly IRabbitMqMessageReceiver _rabbitMqMessageReceiverForTickets;
        private readonly IRabbitMqMessageReceiver _rabbitMqMessageReceiverForTicketCancels;
        private readonly IRabbitMqMessageReceiver _rabbitMqMessageReceiverForTicketCashouts;

        private readonly ConcurrentDictionary<string, AutoResetEvent> _autoResetEventsForBlockingRequests;
        private readonly ConcurrentDictionary<string, ISdkTicket> _responsesFromBlockingRequests;
        private readonly MemoryCache _ticketsForNonBlockingRequests;
        private readonly object _lockForTicketsForNonBlockingRequestsCache;
        private CacheItemPolicy _cacheItemPolicyForTicketsForNonBlockingRequestsCache;

        private readonly IMtsClientApi _mtsClientApi;

        /// <summary>
        /// A <see cref="IUnityContainer"/> used to resolve
        /// </summary>
        private readonly IUnityContainer _unityContainer;

        /// <summary>
        /// Value indicating whether the instance has been disposed
        /// </summary>
        private bool _isDisposed;

        /// <summary>
        /// Value indicating whether the current <see cref="IMtsSdk"/> is already opened
        /// 0 indicates false; 1 indicates true
        /// </summary>
        private long _isOpened;

        /// <summary>
        /// Gets a value indicating whether the current instance is opened
        /// </summary>
        public bool IsOpened => _isOpened == 1;

        /// <summary>
        /// The <see cref="ISdkConfiguration"/> representing sdk configuration
        /// </summary>
        private readonly ISdkConfigurationInternal _config;

        /// <summary>
        /// Raised when the current instance of <see cref="IMtsSdk" /> received <see cref="ITicketResponse" />
        /// </summary>
        public event EventHandler<TicketResponseReceivedEventArgs> TicketResponseReceived;

        /// <summary>
        /// Raised when the current instance of <see cref="IMtsSdk" /> did not receive <see cref="ITicketResponse" /> within timeout
        /// </summary>
        public event EventHandler<TicketMessageEventArgs> TicketResponseTimedOut;

        /// <summary>
        /// Raised when the attempt to send ticket failed
        /// </summary>
        public event EventHandler<TicketSendFailedEventArgs> SendTicketFailed;

        /// <summary>
        /// Raised when a message which cannot be parsed is received
        /// </summary>
        public event EventHandler<UnparsableMessageEventArgs> UnparsableTicketResponseReceived;

        /// <summary>
        /// Gets the <see cref="IBuilderFactory" /> instance used to construct builders with some
        /// of the properties pre-loaded from the configuration
        /// </summary>
        public IBuilderFactory BuilderFactory { get; }

        /// <summary>
        /// Constructs a new instance of the <see cref="MtsSdk"/> class
        /// </summary>
        /// <param name="config">A <see cref="ISdkConfiguration"/> instance representing feed configuration</param>
        public MtsSdk(ISdkConfiguration config)
        {
            Contract.Requires(config != null);

            LogInit();

            _isDisposed = false;
            _isOpened = 0;

            _unityContainer = new UnityContainer();
            _unityContainer.RegisterTypes(config);

            _config = _unityContainer.Resolve<ISdkConfigurationInternal>();
            _connectionValidator = _unityContainer.Resolve<ConnectionValidator>();

            BuilderFactory = _unityContainer.Resolve<IBuilderFactory>();
            _ticketPublisherFactory = _unityContainer.Resolve<ITicketSenderFactory>();

            _entitiesMapper = _unityContainer.Resolve<EntitiesMapper>();

            _rabbitMqMessageReceiverForTickets = _unityContainer.Resolve<IRabbitMqMessageReceiver>("TicketResponseMessageReceiver");
            _rabbitMqMessageReceiverForTicketCancels = _unityContainer.Resolve<IRabbitMqMessageReceiver>("TicketCancelResponseMessageReceiver");
            _rabbitMqMessageReceiverForTicketCashouts = _unityContainer.Resolve<IRabbitMqMessageReceiver>("TicketCashoutResponseMessageReceiver");

            _mtsClientApi = _unityContainer.Resolve<IMtsClientApi>();

            _autoResetEventsForBlockingRequests = new ConcurrentDictionary<string, AutoResetEvent>();
            _responsesFromBlockingRequests = new ConcurrentDictionary<string, ISdkTicket>();
            _ticketsForNonBlockingRequests = new MemoryCache("TicketsForNonBlockingRequests");
            _lockForTicketsForNonBlockingRequestsCache = new object();

            foreach (var t in Enum.GetValues(typeof(SdkTicketType)))
            {
                var publisher = _ticketPublisherFactory.GetTicketSender((SdkTicketType)t);
                if (publisher != null)
                {
                    publisher.TicketSendFailed += PublisherOnTicketSendFailed;
                }
            }
        }

        private void PublisherOnTicketSendFailed(object sender, TicketSendFailedEventArgs ticketSendFailedEventArgs)
        {
            ExecutionLog.Info($"Publish of ticket {ticketSendFailedEventArgs.TicketId} failed.");
            // first clean it from awaiting ticket response
            lock (_lockForTicketsForNonBlockingRequestsCache)
            {
                if (_ticketsForNonBlockingRequests.Contains(ticketSendFailedEventArgs.TicketId))
                {
                    _ticketsForNonBlockingRequests.Remove(ticketSendFailedEventArgs.TicketId);
                }
            }
            SendTicketFailed?.Invoke(sender, ticketSendFailedEventArgs);
        }

        /// <summary>
        /// Defines invariant members of the class
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_config != null);
            Contract.Invariant(_ticketPublisherFactory != null);
            Contract.Invariant(InteractionLog != null);
            Contract.Invariant(ExecutionLog != null);
            Contract.Invariant(_unityContainer != null);
            Contract.Invariant(_connectionValidator != null);
            Contract.Invariant(_entitiesMapper != null);
            Contract.Invariant(_rabbitMqMessageReceiverForTickets != null);
            Contract.Invariant(_rabbitMqMessageReceiverForTicketCancels != null);
            Contract.Invariant(_rabbitMqMessageReceiverForTicketCashouts != null);
            Contract.Invariant(_autoResetEventsForBlockingRequests != null);
            Contract.Invariant(_responsesFromBlockingRequests != null);
        }

        /// <summary>
        /// Disposes the current instance and resources associated with it
        /// </summary>
        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Constructs a <see cref="ISdkConfiguration" /> instance with information read from application configuration file
        /// </summary>
        /// <returns>A <see cref="ISdkConfiguration" /> instance read from application configuration file</returns>
        /// <exception cref="InvalidOperationException">The configuration could not be loaded, or the requested section does not exist in the config file</exception>
        /// <exception cref="ConfigurationErrorsException">The section read from the configuration file is not valid</exception>
        public static ISdkConfiguration GetConfiguration()
        {
            Contract.Ensures(Contract.Result<ISdkConfiguration>() != null);
            var section = SdkConfigurationSection.GetSection();
            return new SdkConfiguration(section);
        }

        /// <summary>
        /// Constructs a <see cref="ISdkConfiguration" /> instance from provided information
        /// </summary>
        /// <param name="username">The username used when establishing connection to the AMQP broker</param>
        /// <param name="password">The password used when establishing connection to the AMQP broker</param>
        /// <param name="host">A value specifying the host name of the AMQP broker</param>
        /// <param name="vhost">A value specifying the virtual host name of the AMQP broker</param>
        /// <param name="useSsl">A value specifying whether the connection to AMQP broker should use SSL encryption</param>
        /// <param name="accessToken">Gets the access token for the UF feed (only necessary if UF selections will be build)</param>
        /// <returns>A <see cref="ISdkConfiguration" /> instance created from provided information</returns>
        [Obsolete("Instead use SdkConfigurationBuilder")]
        public static ISdkConfiguration CreateConfiguration(string username,
                                                            string password,
                                                            string host,
                                                            string vhost = null,
                                                            bool useSsl = true,
                                                            string accessToken = null)
        {
            return new SdkConfiguration(username, password, host, vhost, useSsl, 1, 0, 0, null, null, accessToken);
        }

        /// <summary>
        /// Creates the <see cref="ISdkConfigurationBuilder"/> for building the <see cref="ISdkConfiguration"/>
        /// </summary>
        /// <returns>A <see cref="ISdkConfigurationBuilder"/> to be used to create <see cref="ISdkConfiguration"/></returns>
        public static ISdkConfigurationBuilder CreateConfigurationBuilder()
        {
            return new SdkConfigurationBuilder();
        }

        /// <summary>
        /// Closes the current <see cref="IMtsSdk"/> instance and disposes resources used by it
        /// </summary>
        public void Close()
        {
            ((IDisposable)this).Dispose();
        }

        /// <summary>
        /// Disposes the current instance and resources associated with it
        /// </summary>
        /// <param name="disposing">Value indicating whether the managed resources should also be disposed</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            _rabbitMqMessageReceiverForTickets.MqMessageReceived -= OnMqMessageReceived;
            //_rabbitMqMessageReceiverForTickets.MqMessageDeserializationFailed -= OnMqMessageDeserializationFailed;
            _rabbitMqMessageReceiverForTickets.Close();

            _rabbitMqMessageReceiverForTicketCancels.MqMessageReceived -= OnMqMessageReceived;
            //_rabbitMqMessageReceiverForTicketCancels.MqMessageDeserializationFailed -= OnMqMessageDeserializationFailed;
            _rabbitMqMessageReceiverForTicketCancels.Close();

            _rabbitMqMessageReceiverForTicketCashouts.MqMessageReceived -= OnMqMessageReceived;
            //_rabbitMqMessageReceiverForTicketCashouts.MqMessageDeserializationFailed -= OnMqMessageDeserializationFailed;
            _rabbitMqMessageReceiverForTicketCashouts.Close();

            _ticketPublisherFactory.Close();

            foreach (var item in _autoResetEventsForBlockingRequests)
            {
                AutoResetEvent arEvent;
                _autoResetEventsForBlockingRequests.TryRemove(item.Key, out arEvent);
                ExecutionLog.Debug($"Disposing AutoResetEvent for TicketId: {item.Key}.");
                arEvent.Dispose();
            }

            _responsesFromBlockingRequests.Clear();

            if (disposing)
            {
                try
                {
                    _unityContainer.Dispose();
                }
                catch (Exception ex)
                {
                    ExecutionLog.Warn("An exception has occurred while disposing the feed instance. Exception: ", ex);
                }
            }

            _isDisposed = true;
            _isOpened = 0;
        }

        /// <summary>
        /// Opens the current feed
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">The feed is already disposed</exception>
        /// <exception cref="System.InvalidOperationException">The feed is already opened</exception>
        /// <exception cref="CommunicationException"> Connection to the message broker failed, Probable Reason={Invalid or expired token}</exception>
        public void Open()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(ToString());
            }

            if (Interlocked.CompareExchange(ref _isOpened, 1, 0) != 0)
            {
                throw new InvalidOperationException("The feed is already opened");
            }

            try
            {
                _rabbitMqMessageReceiverForTickets.MqMessageReceived += OnMqMessageReceived;
                //_rabbitMqMessageReceiverForTickets.MqMessageDeserializationFailed += OnMqMessageDeserializationFailed;
                _rabbitMqMessageReceiverForTickets.Open();

                _rabbitMqMessageReceiverForTicketCancels.MqMessageReceived += OnMqMessageReceived;
                //_rabbitMqMessageReceiverForTicketCancels.MqMessageDeserializationFailed += OnMqMessageDeserializationFailed;
                _rabbitMqMessageReceiverForTicketCancels.Open();

                _rabbitMqMessageReceiverForTicketCashouts.MqMessageReceived += OnMqMessageReceived;
                //_rabbitMqMessageReceiverForTicketCashouts.MqMessageDeserializationFailed += OnMqMessageDeserializationFailed;
                _rabbitMqMessageReceiverForTicketCashouts.Open();

                _ticketPublisherFactory.Open();
            }
            catch (CommunicationException ex)
            {
                // this should really almost never happen
                var result = _connectionValidator.ValidateConnection();
                if (result == ConnectionValidationResult.Success)
                {
                    throw new CommunicationException(
                        "Connection to the API failed, Probable Reason={Invalid or expired token}",
                        $"{_config.Host}:{_config.Port}",
                        ex.InnerException);
                }

                var publicIp = _connectionValidator.GetPublicIp();
                throw new CommunicationException(
                    $"Connection to the API failed. Probable Reason={result.Message}, Public IP={publicIp}",
                    $"{_config.Host}:{_config.Port}",
                    ex);
            }
            catch (BrokerUnreachableException ex)
            {
                // this should really almost never happen
                var result = _connectionValidator.ValidateConnection();
                if (result == ConnectionValidationResult.Success)
                {
                    throw new CommunicationException(
                        "Connection to the message broker failed, Probable Reason={Invalid or expired token}",
                        $"{_config.Host}:{_config.Port}",
                        ex.InnerException);
                }

                var publicIp = _connectionValidator.GetPublicIp();
                throw new CommunicationException(
                    $"Connection to the message broker failed. Probable Reason={result.Message}, Public IP={publicIp}",
                    $"{_config.Host}:{_config.Port}",
                    ex);
            }
            ExecutionLog.Info("MtsSdk instance opened.");
        }

        private void OnMqMessageReceived(object sender, MessageReceivedEventArgs eventArgs)
        {
            var stopwatch = Stopwatch.StartNew();

            if (ExecutionLog.IsDebugEnabled)
            {
                ExecutionLog.Debug($"Received ticket response for correlationId={eventArgs.CorrelationId} and routingKey={eventArgs.RoutingKey}. JSON={eventArgs.JsonBody}");
            }
            else
            {
                ExecutionLog.Info($"Received ticket response for correlationId={eventArgs.CorrelationId}.");
            }

            ISdkTicket ticket;
            try
            {
                ticket = _entitiesMapper.GetTicketResponseFromJson(eventArgs.JsonBody, eventArgs.RoutingKey, eventArgs.ResponseType, eventArgs.CorrelationId, eventArgs.AdditionalInfo);
            }
            catch (Exception e)
            {
                ExecutionLog.Debug("Received message deserialization failed.", e);
                //deserialization failed
                OnMqMessageDeserializationFailed(sender, new MessageDeserializationFailedEventArgs(Encoding.UTF8.GetBytes(eventArgs.JsonBody)));
                return;
            }

            // first clean it from awaiting ticket response
            lock (_lockForTicketsForNonBlockingRequestsCache)
            {
                if (_ticketsForNonBlockingRequests.Contains(ticket.TicketId))
                {
                    _ticketsForNonBlockingRequests.Remove(ticket.TicketId);
                }
            }

            //ExecutionLog.Debug($"Processing ticket response from JSON (time: {stopwatch.ElapsedMilliseconds} ms).");

            // check if it was called from SendBlocking
            if (_autoResetEventsForBlockingRequests.ContainsKey(ticket.TicketId))
            {
                _responsesFromBlockingRequests.TryAdd(ticket.TicketId, ticket);
                ReleaseAutoResetEventFromDictionary(ticket.TicketId);
                return;
            }
            //ExecutionLog.Debug($"Processing ticket response from AutoResetEvent (time: {stopwatch.ElapsedMilliseconds} ms).");

            //else raise event
            var ticketReceivedEventArgs = new TicketResponseReceivedEventArgs(ticket);

            Metric.Context("MtsSdk").Meter("TicketReceived", Unit.Items).Mark(ticketReceivedEventArgs.Type.ToString());

            ExecutionLog.Info($"Invoking TicketResponseReceived event for {eventArgs.ResponseType} response with correlationId={eventArgs.CorrelationId}.");

            TicketResponseReceived?.Invoke(this, ticketReceivedEventArgs);

            stopwatch.Stop();
            ExecutionLog.Info($"Processing TicketResponseReceived event for {eventArgs.ResponseType} response with correlationId={eventArgs.CorrelationId} finished in {stopwatch.ElapsedMilliseconds} ms.");
        }

        // ReSharper disable once UnusedParameter.Local
        private void OnMqMessageDeserializationFailed(object sender, MessageDeserializationFailedEventArgs eventArgs)
        {
            var rawData = eventArgs.RawData as byte[] ?? eventArgs.RawData.ToArray();
            var basicMessageData = TicketHelper.ParseUnparsableMsg(rawData);
            ExecutionLog.Info($"Extracted the following data from unparsed message data: [{basicMessageData}], raising OnUnparsableMessageReceived event");
            var dispatchmentEventArgs = new UnparsableMessageEventArgs(basicMessageData);
            Metric.Context("MtsSdk").Meter("TicketDeserializationFailed", Unit.Items).Mark();
            UnparsableTicketResponseReceived?.Invoke(this, dispatchmentEventArgs);
        }

        private int SendTicketBase(ISdkTicket ticket, bool waitForResponse)
        {
            ExecutionLog.Info($"Sending {ticket.GetType().Name} with ticketId: {ticket.TicketId}.");
            var ticketType = TicketHelper.GetTicketTypeFromTicket(ticket);
            var ticketSender = _ticketPublisherFactory.GetTicketSender(ticketType);
            ticketSender.SendTicket(ticket);
            if (waitForResponse)
            {
                return ticketSender.GetCacheTimeout;
            }
            else
            {
                lock (_lockForTicketsForNonBlockingRequestsCache)
                {
                    if (TicketResponseTimedOut != null)
                    {
                        _cacheItemPolicyForTicketsForNonBlockingRequestsCache = new CacheItemPolicy
                                                                                {
                                                                                    AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddMilliseconds(ticketSender.GetCacheTimeout)),
                                                                                    RemovedCallback = RemovedFromCacheForTicketsForNonBlockingRequestsCallback
                                                                                };
                        _ticketsForNonBlockingRequests.Add(ticket.TicketId, ticket, _cacheItemPolicyForTicketsForNonBlockingRequestsCache);
                    }
                }
            }
            return -1;
        }

        private void RemovedFromCacheForTicketsForNonBlockingRequestsCallback(CacheEntryRemovedArguments arguments)
        {
            if (arguments.RemovedReason == CacheEntryRemovedReason.Expired)
            {
                var sendTicket = (ISdkTicket) arguments.CacheItem.Value;
                TicketResponseTimedOut?.Invoke(this, new TicketMessageEventArgs(sendTicket.TicketId, sendTicket, "Ticket response not received within timeout."));
            }
        }

        private ISdkTicket SendTicketBlockingBase(ISdkTicket ticket)
        {
            var stopwatch = Stopwatch.StartNew();
            var responseTimeout = SendTicketBase(ticket, true);

            var autoResetEvent = new AutoResetEvent(false);
            _autoResetEventsForBlockingRequests.TryAdd(ticket.TicketId, autoResetEvent);

            autoResetEvent.WaitOne(TimeSpan.FromMilliseconds(responseTimeout));

            ISdkTicket responseTicket;
            if (_responsesFromBlockingRequests.TryRemove(ticket.TicketId, out responseTicket))
            {
                stopwatch.Stop();
                ReleaseAutoResetEventFromDictionary(ticket.TicketId);
                ExecutionLog.Debug($"Sending in blocking mode and successfully received response for {ticket.GetType().Name} {ticket.TicketId} took {stopwatch.ElapsedMilliseconds} ms.");
                return responseTicket;
            }
            stopwatch.Stop();
            ReleaseAutoResetEventFromDictionary(ticket.TicketId);
            ExecutionLog.Debug($"Sending in blocking mode and waiting for receiving response for {ticket.GetType().Name} {ticket.TicketId} took {stopwatch.ElapsedMilliseconds} ms. Response not received in required timeout.");
            var msg = $"The timeout for receiving response elapsed. Org. {ticket.GetType().Name}: {ticket.TicketId}.";
            ExecutionLog.Info(msg);
            throw new TimeoutException(msg);
        }

        private void ReleaseAutoResetEventFromDictionary(string ticketId)
        {
            AutoResetEvent autoResetEvent;
            if (_autoResetEventsForBlockingRequests.TryRemove(ticketId, out autoResetEvent))
            {
                autoResetEvent.Set();
                autoResetEvent.Dispose();
            }
        }

        /// <summary>
        /// Sends the ticket to the MTS server. The response will raise TicketResponseReceived event
        /// </summary>
        /// <param name="ticket">The <see cref="ISdkTicket"/> to be send</param>
        public void SendTicket(ISdkTicket ticket)
        {
            Metric.Context("MtsSdk").Meter("SendTicket", Unit.Items).Mark();
            InteractionLog.Info($"Called SendTicket with ticketId={ticket.TicketId}.");
            SendTicketBase(ticket, false);
        }

        /// <summary>
        /// Sends the ticket to the MTS server and wait for the response message on the feed
        /// </summary>
        /// <param name="ticket">The <see cref="ITicket"/> to be send</param>
        /// <returns>Returns a <see cref="ITicketResponse" /></returns>
        public ITicketResponse SendTicketBlocking(ITicket ticket)
        {
            Metric.Context("MtsSdk").Meter("SendTicketBlocking", Unit.Items).Mark();
            InteractionLog.Info($"Called SendTicketBlocking with ticketId={ticket.TicketId}.");
            return (ITicketResponse) SendTicketBlockingBase(ticket);
        }

        /// <summary>
        /// Sends the cancel ticket to the MTS server and wait for the response message on the feed
        /// </summary>
        /// <param name="ticket">The <see cref="ITicketCancel"/> to be send</param>
        /// <returns>Returns a <see cref="ITicketCancelResponse" /></returns>
        public ITicketCancelResponse SendTicketCancelBlocking(ITicketCancel ticket)
        {
            Metric.Context("MtsSdk").Meter("SendTicketCancelBlocking", Unit.Items).Mark();
            InteractionLog.Info($"Called SendTicketCancelBlocking with ticketId={ticket.TicketId}.");
            return (ITicketCancelResponse) SendTicketBlockingBase(ticket);
        }

        /// <summary>
        /// Sends the cashout ticket to the MTS server and wait for the response message on the feed
        /// </summary>
        /// <param name="ticket">A <see cref="ITicketCashout" /> to be send</param>
        /// <returns>Returns a <see cref="ITicketCashoutResponse" /></returns>
        public ITicketCashoutResponse SendTicketCashoutBlocking(ITicketCashout ticket)
        {
            Metric.Context("MtsSdk").Meter("SendTicketCashoutBlocking", Unit.Items).Mark();
            InteractionLog.Info($"Called SendTicketCashoutBlocking with ticketId={ticket.TicketId}.");
            return (ITicketCashoutResponse)SendTicketBlockingBase(ticket);
        }

        private static void LogInit()
        {
            var msg = "MtsSdk initialization. Version: " + SdkInfo.GetVersion();
            var logger = SdkLoggerFactory.GetLoggerForFeedTraffic(typeof(MtsSdk));
            logger.Info(msg);
            logger = SdkLoggerFactory.GetLoggerForCache(typeof(MtsSdk));
            logger.Info(msg);
            logger = SdkLoggerFactory.GetLoggerForClientInteraction(typeof(MtsSdk));
            logger.Info(msg);
            logger = SdkLoggerFactory.GetLoggerForRestTraffic(typeof(MtsSdk));
            logger.Info(msg);
            logger = SdkLoggerFactory.GetLoggerForStats(typeof(MtsSdk));
            logger.Info(msg);
            logger = SdkLoggerFactory.GetLoggerForExecution(typeof(MtsSdk));
            logger.Info(msg);
        }

        public IMtsClientApi GetClientApi()
        {
            return _mtsClientApi;
        }
    }
}
