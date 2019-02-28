/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Net.Http;
using System.Runtime.Caching;
using log4net;
using Metrics;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using RabbitMQ.Client;
using Sportradar.MTS.SDK.API.Internal.Mappers;
using Sportradar.MTS.SDK.API.Internal.RabbitMq;
using Sportradar.MTS.SDK.API.Internal.Senders;
using Sportradar.MTS.SDK.Common.Internal;
using Sportradar.MTS.SDK.Common.Internal.Metrics;
using Sportradar.MTS.SDK.Common.Internal.Metrics.Reports;
using Sportradar.MTS.SDK.Common.Internal.Rest;
using Sportradar.MTS.SDK.Common.Log;
using Sportradar.MTS.SDK.Entities;
using Sportradar.MTS.SDK.Entities.Builders;
using Sportradar.MTS.SDK.Entities.Enums;
using Sportradar.MTS.SDK.Entities.Internal;
using Sportradar.MTS.SDK.Entities.Internal.Builders;
using Sportradar.MTS.SDK.Entities.Internal.Cache;
using Sportradar.MTS.SDK.Entities.Internal.REST;
using Sportradar.MTS.SDK.Entities.Internal.REST.ClientApi;
using Sportradar.MTS.SDK.Entities.Internal.REST.Dto;

// ReSharper disable RedundantTypeArgumentsOfMethod

namespace Sportradar.MTS.SDK.API.Internal
{
    internal static class SdkUnityBootstrapper
    {
        private static readonly ILog Log = SdkLoggerFactory.GetLogger(typeof(SdkUnityBootstrapper));
        private const int RestConnectionFailureLimit = 3;
        private const int RestConnectionFailureTimeoutInSec = 12;
        private static string _environment;
        private static readonly CultureInfo DefaultCulture = new CultureInfo("en");

        public static void RegisterTypes(this IUnityContainer container, ISdkConfiguration userConfig)
        {
            Contract.Requires(container != null);
            Contract.Requires(userConfig != null);

            RegisterBaseClasses(container, userConfig);

            RegisterRabbitMqTypes(container, userConfig, _environment);

            RegisterTicketSenders(container);

            RegisterMarketDescriptionCache(container, userConfig);

            RegisterSdkStatisticsWriter(container, userConfig);

            RegisterClientApi(container, userConfig);
        }

        private static void RegisterBaseClasses(IUnityContainer container, ISdkConfiguration config)
        {
            container.RegisterInstance(config, new ContainerControlledLifetimeManager());

            container.RegisterType<ISdkConfigurationInternal, SdkConfigurationInternal>(new ContainerControlledLifetimeManager());
            var configInternal = new SdkConfigurationInternal(config);
            container.RegisterInstance(configInternal);

            if (configInternal.Host.Contains("tradinggate"))
            {
                _environment = "PROD";
            }
            else if (configInternal.Host.Contains("integration"))
            {
                _environment = "CI";
            }
            else
            {
                _environment = "CUSTOM";
            }

            container.RegisterType<IRabbitServer>(new ContainerControlledLifetimeManager());
            var rabbitServer = new RabbitServer(configInternal);
            container.RegisterInstance<IRabbitServer>(rabbitServer);

            container.RegisterType<ConnectionValidator, ConnectionValidator>(new ContainerControlledLifetimeManager());

            container.RegisterType<IConnectionFactory, ConfiguredConnectionFactory>(new ContainerControlledLifetimeManager());

            container.RegisterType<IChannelFactory, ChannelFactory>(new ContainerControlledLifetimeManager());

            container.RegisterInstance<ISequenceGenerator>(new IncrementalSequenceGenerator(), new ContainerControlledLifetimeManager());

            //register common types
            container.RegisterType<HttpClient, HttpClient>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor());

            var seed = (int)DateTime.Now.Ticks;
            var rand = new Random(seed);
            var value = rand.Next();
            Log.Info($"Initializing sequence generator with MinValue={value}, MaxValue={long.MaxValue}");
            container.RegisterType<ISequenceGenerator, IncrementalSequenceGenerator>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    (long)value,
                    long.MaxValue));

            container.RegisterType<HttpDataFetcher, HttpDataFetcher>("Base",
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new ResolvedParameter<HttpClient>(),
                    config.AccessToken ?? string.Empty,
                    RestConnectionFailureLimit,
                    RestConnectionFailureTimeoutInSec));

            container.RegisterType<LogHttpDataFetcher, LogHttpDataFetcher>("Base",
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new ResolvedParameter<HttpClient>(),
                    config.AccessToken ?? string.Empty,
                    new ResolvedParameter<ISequenceGenerator>(),
                    RestConnectionFailureLimit,
                    RestConnectionFailureTimeoutInSec));

            var logFetcher = container.Resolve<LogHttpDataFetcher>("Base");
            container.RegisterInstance<IDataFetcher>("Base", logFetcher, new ContainerControlledLifetimeManager());
            container.RegisterInstance<IDataPoster>("Base", logFetcher, new ContainerControlledLifetimeManager());
        }

        private static void RegisterRabbitMqTypes(IUnityContainer container, ISdkConfiguration config, string environment)
        {
            Contract.Assume(container.Resolve<IChannelFactory>() != null);

            container.RegisterType<IRabbitMqChannelSettings, RabbitMqChannelSettings>(new ContainerControlledLifetimeManager());
            container.RegisterInstance<IRabbitMqChannelSettings>("TicketChannelSettings", new RabbitMqChannelSettings(true, config.ExclusiveConsumer, publishQueueTimeoutInMs: config.TicketResponseTimeout));
            container.RegisterInstance<IRabbitMqChannelSettings>("TicketCancellationChannelSettings", new RabbitMqChannelSettings(true, config.ExclusiveConsumer, publishQueueTimeoutInMs: config.TicketCancellationResponseTimeout));
            container.RegisterInstance<IRabbitMqChannelSettings>("TicketCashoutChannelSettings", new RabbitMqChannelSettings(true, config.ExclusiveConsumer, publishQueueTimeoutInMs: config.TicketCashoutResponseTimeout));
            container.RegisterInstance<IRabbitMqChannelSettings>("TicketNonSrSettleChannelSettings", new RabbitMqChannelSettings(true, config.ExclusiveConsumer, publishQueueTimeoutInMs: config.TicketNonSrSettleResponseTimeout));

            var rootExchangeName = config.VirtualHost.Replace("/", string.Empty);
            container.RegisterType<IMtsChannelSettings, MtsChannelSettings>(new ContainerControlledLifetimeManager());
            var mtsTicketChannelSettings = MtsChannelSettings.GetTicketChannelSettings(rootExchangeName, config.Username, config.NodeId, environment);
            var mtsTicketCancelChannelSettings = MtsChannelSettings.GetTicketCancelChannelSettings(rootExchangeName, config.Username, config.NodeId, environment);
            var mtsTicketAckChannelSettings = MtsChannelSettings.GetTicketAckChannelSettings(rootExchangeName, config.Username, config.NodeId, environment);
            var mtsTicketCancelAckChannelSettings = MtsChannelSettings.GetTicketCancelAckChannelSettings(rootExchangeName, config.Username, config.NodeId, environment);
            var mtsTicketReofferCancelChannelSettings = MtsChannelSettings.GetTicketReofferCancelChannelSettings(rootExchangeName, config.Username, config.NodeId, environment);
            var mtsTicketCashoutChannelSettings = MtsChannelSettings.GetTicketCashoutChannelSettings(rootExchangeName, config.Username, config.NodeId, environment);
            var mtsTicketNonSrSettleChannelSettings = MtsChannelSettings.GetTicketNonSrSettleChannelSettings(rootExchangeName, config.Username, config.NodeId, environment);

            var mtsTicketResponseChannelSettings = MtsChannelSettings.GetTicketResponseChannelSettings(rootExchangeName, config.Username, config.NodeId, environment);
            var mtsTicketCancelResponseChannelSettings = MtsChannelSettings.GetTicketCancelResponseChannelSettings(rootExchangeName, config.Username, config.NodeId, environment);
            var mtsTicketCashoutResponseChannelSettings = MtsChannelSettings.GetTicketCashoutResponseChannelSettings(rootExchangeName, config.Username, config.NodeId, environment);
            var mtsTicketNonSrSettleResponseChannelSettings = MtsChannelSettings.GetTicketNonSrSettleResponseChannelSettings(rootExchangeName, config.Username, config.NodeId, environment);

            container.RegisterInstance<IMtsChannelSettings>("TicketChannelSettings", mtsTicketChannelSettings);
            container.RegisterInstance<IMtsChannelSettings>("TicketCancelChannelSettings", mtsTicketCancelChannelSettings);
            container.RegisterInstance<IMtsChannelSettings>("TicketAckChannelSettings", mtsTicketAckChannelSettings);
            container.RegisterInstance<IMtsChannelSettings>("TicketCancelAckChannelSettings", mtsTicketCancelAckChannelSettings);
            container.RegisterInstance<IMtsChannelSettings>("TicketReofferCancelChannelSettings", mtsTicketReofferCancelChannelSettings);
            container.RegisterInstance<IMtsChannelSettings>("TicketCashoutChannelSettings", mtsTicketCashoutChannelSettings);
            container.RegisterInstance<IMtsChannelSettings>("TicketNonSrSettleChannelSettings", mtsTicketNonSrSettleChannelSettings);

            container.RegisterInstance<IMtsChannelSettings>("TicketResponseChannelSettings", mtsTicketResponseChannelSettings);
            container.RegisterInstance<IMtsChannelSettings>("TicketCancelResponseChannelSettings", mtsTicketCancelResponseChannelSettings);
            container.RegisterInstance<IMtsChannelSettings>("TicketCashoutResponseChannelSettings", mtsTicketCashoutResponseChannelSettings);
            container.RegisterInstance<IMtsChannelSettings>("TicketNonSrSettleResponseChannelSettings", mtsTicketNonSrSettleResponseChannelSettings);

            container.RegisterType<IRabbitMqConsumerChannel, RabbitMqConsumerChannel>(new HierarchicalLifetimeManager());
            var ticketResponseConsumerChannel = new RabbitMqConsumerChannel(container.Resolve<IChannelFactory>(),
                                                                            container.Resolve<IMtsChannelSettings>("TicketResponseChannelSettings"),
                                                                            container.Resolve<IRabbitMqChannelSettings>("TicketChannelSettings"));
            var ticketCancelResponseConsumerChannel = new RabbitMqConsumerChannel(container.Resolve<IChannelFactory>(),
                                                                            container.Resolve<IMtsChannelSettings>("TicketCancelResponseChannelSettings"),
                                                                            container.Resolve<IRabbitMqChannelSettings>("TicketCancellationChannelSettings"));
            var ticketCashoutResponseConsumerChannel = new RabbitMqConsumerChannel(container.Resolve<IChannelFactory>(),
                                                                            container.Resolve<IMtsChannelSettings>("TicketCashoutResponseChannelSettings"),
                                                                            container.Resolve<IRabbitMqChannelSettings>("TicketCashoutChannelSettings"));
            var ticketNonSrSettleResponseConsumerChannel = new RabbitMqConsumerChannel(container.Resolve<IChannelFactory>(),
                                                                            container.Resolve<IMtsChannelSettings>("TicketNonSrSettleResponseChannelSettings"),
                                                                            container.Resolve<IRabbitMqChannelSettings>("TicketNonSrSettleChannelSettings"));
            container.RegisterInstance<IRabbitMqConsumerChannel>("TicketConsumerChannel", ticketResponseConsumerChannel);
            container.RegisterInstance<IRabbitMqConsumerChannel>("TicketCancelConsumerChannel", ticketCancelResponseConsumerChannel);
            container.RegisterInstance<IRabbitMqConsumerChannel>("TicketCashoutConsumerChannel", ticketCashoutResponseConsumerChannel);
            container.RegisterInstance<IRabbitMqConsumerChannel>("TicketNonSrSettleConsumerChannel", ticketNonSrSettleResponseConsumerChannel);

            container.RegisterType<IRabbitMqMessageReceiver, RabbitMqMessageReceiver>(new HierarchicalLifetimeManager());
            container.RegisterInstance<IRabbitMqMessageReceiver>("TicketResponseMessageReceiver", new RabbitMqMessageReceiver(ticketResponseConsumerChannel, TicketResponseType.Ticket));
            container.RegisterInstance<IRabbitMqMessageReceiver>("TicketCancelResponseMessageReceiver", new RabbitMqMessageReceiver(ticketCancelResponseConsumerChannel, TicketResponseType.TicketCancel));
            container.RegisterInstance<IRabbitMqMessageReceiver>("TicketCashoutResponseMessageReceiver", new RabbitMqMessageReceiver(ticketCashoutResponseConsumerChannel, TicketResponseType.TicketCashout));
            container.RegisterInstance<IRabbitMqMessageReceiver>("TicketNonSrSettleResponseMessageReceiver", new RabbitMqMessageReceiver(ticketNonSrSettleResponseConsumerChannel, TicketResponseType.TicketNonSrSettle));

            container.RegisterType<IRabbitMqPublisherChannel, RabbitMqPublisherChannel>(new HierarchicalLifetimeManager());
            var ticketPC = new RabbitMqPublisherChannel(container.Resolve<IChannelFactory>(),
                                                        mtsTicketChannelSettings,
                                                        container.Resolve<IRabbitMqChannelSettings>("TicketChannelSettings"));
            var ticketAckPC = new RabbitMqPublisherChannel(container.Resolve<IChannelFactory>(),
                                                        mtsTicketAckChannelSettings,
                                                        container.Resolve<IRabbitMqChannelSettings>("TicketChannelSettings"));
            var ticketCancelPC = new RabbitMqPublisherChannel(container.Resolve<IChannelFactory>(),
                                                        mtsTicketCancelChannelSettings,
                                                        container.Resolve<IRabbitMqChannelSettings>("TicketCancellationChannelSettings"));
            var ticketCancelAckPC = new RabbitMqPublisherChannel(container.Resolve<IChannelFactory>(),
                                                        mtsTicketCancelAckChannelSettings,
                                                        container.Resolve<IRabbitMqChannelSettings>("TicketCancellationChannelSettings"));
            var ticketReofferCancelPC = new RabbitMqPublisherChannel(container.Resolve<IChannelFactory>(),
                                                        mtsTicketReofferCancelChannelSettings,
                                                        container.Resolve<IRabbitMqChannelSettings>("TicketCancellationChannelSettings"));
            var ticketCashoutPC = new RabbitMqPublisherChannel(container.Resolve<IChannelFactory>(),
                                                        mtsTicketCashoutChannelSettings,
                                                        container.Resolve<IRabbitMqChannelSettings>("TicketCashoutChannelSettings"));
            var ticketNonSrSettlePC = new RabbitMqPublisherChannel(container.Resolve<IChannelFactory>(),
                                                        mtsTicketNonSrSettleChannelSettings,
                                                        container.Resolve<IRabbitMqChannelSettings>("TicketNonSrSettleChannelSettings"));
            container.RegisterInstance<IRabbitMqPublisherChannel>("TicketPublisherChannel", ticketPC);
            container.RegisterInstance<IRabbitMqPublisherChannel>("TicketAckPublisherChannel", ticketAckPC);
            container.RegisterInstance<IRabbitMqPublisherChannel>("TicketCancelPublisherChannel", ticketCancelPC);
            container.RegisterInstance<IRabbitMqPublisherChannel>("TicketCancelAckPublisherChannel", ticketCancelAckPC);
            container.RegisterInstance<IRabbitMqPublisherChannel>("TicketReofferCancelPublisherChannel", ticketReofferCancelPC);
            container.RegisterInstance<IRabbitMqPublisherChannel>("TicketCashoutPublisherChannel", ticketCashoutPC);
            container.RegisterInstance<IRabbitMqPublisherChannel>("TicketNonSrSettlePublisherChannel", ticketNonSrSettlePC);
        }

        private static void RegisterTicketSenders(IUnityContainer container)
        {
            var ticketCache = new ConcurrentDictionary<string, TicketCacheItem>();

            container.RegisterType<ITicketSender>(new ContainerControlledLifetimeManager());
            var ticketSender = new TicketSender(new TicketMapper(),
                                                container.Resolve<IRabbitMqPublisherChannel>("TicketPublisherChannel"),
                                                ticketCache,
                                                container.Resolve<IMtsChannelSettings>("TicketChannelSettings"),
                                                container.Resolve<IRabbitMqChannelSettings>("TicketChannelSettings").PublishQueueTimeoutInMs);
            var ticketAckSender = new TicketAckSender(new TicketAckMapper(),
                                                container.Resolve<IRabbitMqPublisherChannel>("TicketAckPublisherChannel"),
                                                ticketCache,
                                                container.Resolve<IMtsChannelSettings>("TicketAckChannelSettings"),
                                                container.Resolve<IRabbitMqChannelSettings>("TicketChannelSettings").PublishQueueTimeoutInMs);
            var ticketCancelSender = new TicketCancelSender(new TicketCancelMapper(),
                                                container.Resolve<IRabbitMqPublisherChannel>("TicketCancelPublisherChannel"),
                                                ticketCache,
                                                container.Resolve<IMtsChannelSettings>("TicketCancelChannelSettings"),
                                                container.Resolve<IRabbitMqChannelSettings>("TicketCancellationChannelSettings").PublishQueueTimeoutInMs);
            var ticketCancelAckSender = new TicketCancelAckSender(new TicketCancelAckMapper(),
                                                container.Resolve<IRabbitMqPublisherChannel>("TicketCancelAckPublisherChannel"),
                                                ticketCache,
                                                container.Resolve<IMtsChannelSettings>("TicketCancelAckChannelSettings"),
                                                container.Resolve<IRabbitMqChannelSettings>("TicketCancellationChannelSettings").PublishQueueTimeoutInMs);
            var ticketReofferCancelSender = new TicketReofferCancelSender(new TicketReofferCancelMapper(),
                                                container.Resolve<IRabbitMqPublisherChannel>("TicketReofferCancelPublisherChannel"),
                                                ticketCache,
                                                container.Resolve<IMtsChannelSettings>("TicketReofferCancelChannelSettings"),
                                                container.Resolve<IRabbitMqChannelSettings>("TicketCancellationChannelSettings").PublishQueueTimeoutInMs);
            var ticketCashoutSender = new TicketCashoutSender(new TicketCashoutMapper(),
                                                container.Resolve<IRabbitMqPublisherChannel>("TicketCashoutPublisherChannel"),
                                                ticketCache,
                                                container.Resolve<IMtsChannelSettings>("TicketCashoutChannelSettings"),
                                                container.Resolve<IRabbitMqChannelSettings>("TicketCashoutChannelSettings").PublishQueueTimeoutInMs);
            var ticketNonSrSettleSender = new TicketNonSrSettleSender(new TicketNonSrSettleMapper(),
                                                container.Resolve<IRabbitMqPublisherChannel>("TicketNonSrSettlePublisherChannel"),
                                                ticketCache,
                                                container.Resolve<IMtsChannelSettings>("TicketNonSrSettleChannelSettings"),
                                                container.Resolve<IRabbitMqChannelSettings>("TicketNonSrSettleChannelSettings").PublishQueueTimeoutInMs);
            container.RegisterInstance<ITicketSender>("TicketSender", ticketSender);
            container.RegisterInstance<ITicketSender>("TicketAckSender", ticketAckSender);
            container.RegisterInstance<ITicketSender>("TicketCancelSender", ticketCancelSender);
            container.RegisterInstance<ITicketSender>("TicketCancelAckSender", ticketCancelAckSender);
            container.RegisterInstance<ITicketSender>("TicketReofferCancelSender", ticketReofferCancelSender);
            container.RegisterInstance<ITicketSender>("TicketCashoutSender", ticketCashoutSender);
            container.RegisterInstance<ITicketSender>("TicketNonSrSettleSender", ticketNonSrSettleSender);

            var senders = new Dictionary<SdkTicketType, ITicketSender>
            {
                {SdkTicketType.Ticket, container.Resolve<ITicketSender>("TicketSender")},
                {SdkTicketType.TicketAck, container.Resolve<ITicketSender>("TicketAckSender")},
                {SdkTicketType.TicketCancel, container.Resolve<ITicketSender>("TicketCancelSender")},
                {SdkTicketType.TicketCancelAck, container.Resolve<ITicketSender>("TicketCancelAckSender")},
                {SdkTicketType.TicketReofferCancel, container.Resolve<ITicketSender>("TicketReofferCancelSender")},
                {SdkTicketType.TicketCashout, container.Resolve<ITicketSender>("TicketCashoutSender")},
                {SdkTicketType.TicketNonSrSettle, container.Resolve<ITicketSender>("TicketNonSrSettleSender")},
            };

            var senderFactory = new TicketSenderFactory(senders);
            container.RegisterType<ITicketSenderFactory, TicketSenderFactory>(new ContainerControlledLifetimeManager());
            container.RegisterInstance(senderFactory);

            var entityMapper = new EntitiesMapper(ticketAckSender, ticketCancelAckSender);
            container.RegisterType<EntitiesMapper>(new ContainerControlledLifetimeManager());
            container.RegisterInstance(entityMapper);
        }

        private static void RegisterSdkStatisticsWriter(IUnityContainer container, ISdkConfiguration config)
        {
            var x = container.ResolveAll<IRabbitMqMessageReceiver>();
            var statusProviders = new List<IHealthStatusProvider>();
            x.ForEach(f=>statusProviders.Add((IHealthStatusProvider)f));

            container.RegisterType<MetricsReporter, MetricsReporter>(new ContainerControlledLifetimeManager(),
                                                                     new InjectionConstructor(MetricsReportPrintMode.Normal, 2, true));

            var metricReporter = container.Resolve<MetricsReporter>();

            Metric.Config.WithAllCounters().WithReporting(rep => rep.WithReport(metricReporter, TimeSpan.FromSeconds(config.StatisticsTimeout)));

            container.RegisterInstance(metricReporter, new ContainerControlledLifetimeManager());

            foreach (var sp in statusProviders)
            {
                sp.RegisterHealthCheck();
            }
        }

        private static void RegisterMarketDescriptionCache(IUnityContainer container, ISdkConfiguration config)
        {
            var configInternal = container.Resolve<ISdkConfigurationInternal>();

            // Invariant market description provider
            container.RegisterType<IDeserializer<market_descriptions>, Deserializer<market_descriptions>>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISingleTypeMapperFactory<market_descriptions, IEnumerable<MarketDescriptionDTO>>, MarketDescriptionsMapperFactory>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDataProvider<IEnumerable<MarketDescriptionDTO>>,
                DataProvider<market_descriptions, IEnumerable<MarketDescriptionDTO>>>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    configInternal.ApiHost + "/v1/descriptions/{0}/markets.xml?include_mappings=true",
                    new ResolvedParameter<IDataFetcher>("Base"),
                    new ResolvedParameter<IDataPoster>("Base"),
                    new ResolvedParameter<IDeserializer<market_descriptions>>(),
                    new ResolvedParameter<ISingleTypeMapperFactory<market_descriptions, IEnumerable<MarketDescriptionDTO>>>()));

            // Cache for invariant markets
            container.RegisterInstance(
                "InvariantMarketDescriptionCache_Cache",
                new MemoryCache("invariantMarketsDescriptionsCache"),
                new ContainerControlledLifetimeManager());

            // Timer for invariant markets
            container.RegisterType<ITimer, SdkTimer>(
                "InvariantMarketCacheTimer",
                new HierarchicalLifetimeManager(),
                new InjectionConstructor(
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromHours(6)));

            // Invariant market cache
            container.RegisterType<IMarketDescriptionCache, MarketDescriptionCache>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new ResolvedParameter<MemoryCache>("InvariantMarketDescriptionCache_Cache"),
                    new ResolvedParameter<IDataProvider<IEnumerable<MarketDescriptionDTO>>>(),
                    new List<CultureInfo> { DefaultCulture },
                    config.AccessToken ?? string.Empty,
                    TimeSpan.FromHours(4),
                    new CacheItemPolicy { SlidingExpiration = TimeSpan.FromDays(1) }));

            container.RegisterType<IMarketDescriptionProvider, MarketDescriptionProvider>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new ResolvedParameter<IMarketDescriptionCache>(),
                    new List<CultureInfo> { DefaultCulture }));

            container.RegisterType<IBuilderFactory, BuilderFactory>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new ResolvedParameter<ISdkConfigurationInternal>(),
                    new ResolvedParameter<IMarketDescriptionProvider>()));
        }

        private static void RegisterClientApi(IUnityContainer container, ISdkConfiguration userConfig)
        {
            container.RegisterType<HttpDataFetcher, HttpDataFetcher>("MtsClientApi",
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new ResolvedParameter<HttpClient>(),
                    string.Empty,
                    RestConnectionFailureLimit,
                    RestConnectionFailureTimeoutInSec));

            container.RegisterType<LogHttpDataFetcher, LogHttpDataFetcher>("MtsClientApi",
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new ResolvedParameter<HttpClient>(),
                    string.Empty,
                    new ResolvedParameter<ISequenceGenerator>(),
                    RestConnectionFailureLimit,
                    RestConnectionFailureTimeoutInSec));

            var logFetcher = container.Resolve<LogHttpDataFetcher>("MtsClientApi");
            container.RegisterInstance<IDataFetcher>("MtsClientApi", logFetcher, new ContainerControlledLifetimeManager());
            container.RegisterInstance<IDataPoster>("MtsClientApi", logFetcher, new ContainerControlledLifetimeManager());

            container.RegisterType<IDeserializer<KeycloakAuthorization>, Entities.Internal.JsonDeserializer<KeycloakAuthorization>>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISingleTypeMapperFactory<KeycloakAuthorization, KeycloakAuthorizationDTO>, KeycloakAuthorizationMapperFactory>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDataProvider<KeycloakAuthorizationDTO>,
                DataProvider<KeycloakAuthorization, KeycloakAuthorizationDTO>>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    userConfig.KeycloakHost + "/auth/realms/mts/protocol/openid-connect/token",
                    new ResolvedParameter<IDataFetcher>("MtsClientApi"),
                    new ResolvedParameter<IDataPoster>("MtsClientApi"),
                    new ResolvedParameter<IDeserializer<KeycloakAuthorization>>(),
                    new ResolvedParameter<ISingleTypeMapperFactory<KeycloakAuthorization, KeycloakAuthorizationDTO>>()));

            container.RegisterType<IDeserializer<MaxStakeResponse>, Entities.Internal.JsonDeserializer<MaxStakeResponse>>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISingleTypeMapperFactory<MaxStakeResponse, MaxStakeDTO>, MaxStakeMapperFactory>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDataProvider<MaxStakeDTO>,
                DataProvider<MaxStakeResponse, MaxStakeDTO>>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    userConfig.MtsClientApiHost + "/ClientApi/api/maxStake/v1",
                    new ResolvedParameter<IDataFetcher>("MtsClientApi"),
                    new ResolvedParameter<IDataPoster>("MtsClientApi"),
                    new ResolvedParameter<IDeserializer<MaxStakeResponse>>(),
                    new ResolvedParameter<ISingleTypeMapperFactory<MaxStakeResponse, MaxStakeDTO>>()));

            container.RegisterType<IDeserializer<CcfResponse>, Entities.Internal.JsonDeserializer<CcfResponse>>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISingleTypeMapperFactory<CcfResponse, CcfDTO>, CcfMapperFactory>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDataProvider<CcfDTO>,
                DataProvider<CcfResponse, CcfDTO>>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    userConfig.MtsClientApiHost + "/ClientApi/api/ccf/v1?sourceId={0}",
                    new ResolvedParameter<IDataFetcher>("MtsClientApi"),
                    new ResolvedParameter<IDataPoster>("MtsClientApi"),
                    new ResolvedParameter<IDeserializer<CcfResponse>>(),
                    new ResolvedParameter<ISingleTypeMapperFactory<CcfResponse, CcfDTO>>()));

            container.RegisterType<IMtsClientApi, MtsClientApi>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new ResolvedParameter<IDataProvider<MaxStakeDTO>>(),
                    new ResolvedParameter<IDataProvider<CcfDTO>>(),
                    new ResolvedParameter<IDataProvider<KeycloakAuthorizationDTO>>(),
                    new InjectionParameter<string>(userConfig.KeycloakUsername),
                    new InjectionParameter<string>(userConfig.KeycloakPassword),
                    new InjectionParameter<string>(userConfig.KeycloakSecret)
                ));
        }
    }
}

