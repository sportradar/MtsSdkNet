/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Runtime.Caching;
using log4net;
using Sportradar.MTS.SDK.Common.Exceptions;
using Sportradar.MTS.SDK.Common.Internal;
using Sportradar.MTS.SDK.Common.Internal.Rest;
using Sportradar.MTS.SDK.Common.Log;
using Sportradar.MTS.SDK.Entities.Builders;
using Sportradar.MTS.SDK.Entities.Enums;
using Sportradar.MTS.SDK.Entities.Interfaces;
using Sportradar.MTS.SDK.Entities.Internal.Cache;
using Sportradar.MTS.SDK.Entities.Internal.Enums;
using Sportradar.MTS.SDK.Entities.Internal.REST;
using Sportradar.MTS.SDK.Entities.Internal.REST.Dto;
using Sportradar.MTS.SDK.Entities.Internal.TicketImpl;

namespace Sportradar.MTS.SDK.Entities.Internal.Builders
{
    /// <summary>
    /// Class SelectionBuilder
    /// </summary>
    /// <seealso cref="ISelectionBuilder" />
    public class  SelectionBuilder : ISelectionBuilder
    {
        private static readonly ILog ExecutionLog = SdkLoggerFactory.GetLoggerForExecution(typeof(SelectionBuilder));

        private readonly IMarketDescriptionProvider _marketDescriptionProvider;
        private readonly ISdkConfiguration _config;
        /// <summary>
        /// The event identifier
        /// </summary>
        private string _eventId;

        /// <summary>
        /// The identifier
        /// </summary>
        private string _id;

        /// <summary>
        /// The odds
        /// </summary>
        private int _odds;

        /// <summary>
        /// The is banker
        /// </summary>
        private bool _isBanker;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionBuilder"/> class
        /// </summary>
        public SelectionBuilder(IMarketDescriptionProvider marketDescriptionProvider, ISdkConfiguration config)
        {
            Contract.Requires(marketDescriptionProvider != null);
            Contract.Requires(config != null);

            _marketDescriptionProvider = marketDescriptionProvider;
            _isBanker = false;
            _config = config;
        }

        #region Obsolete_members

        /// <summary>
        /// Creates new <see cref="ISelectionBuilder"/>
        /// </summary>
        /// <returns>Returns an <see cref="ISelectionBuilder"/></returns>
        [Obsolete("Method Create() is obsolete. Please use the appropriate method on IBuilderFactory interface which can be obtained through MtsSdk instance")]
        public static ISelectionBuilder Create()
        {
            //TODO: prone to fail in web app
            var configInternal = new SdkConfigurationInternal(new SdkConfiguration(SdkConfigurationSection.GetSection()));
            var value = new Random((int)DateTime.Now.Ticks).Next();
            var dataFetcher = new LogHttpDataFetcher(new HttpClient(),
                                                     configInternal.AccessToken,
                                                     new IncrementalSequenceGenerator(value, long.MaxValue),
                                                     3,
                                                     12);
            var deserializer = new Deserializer<market_descriptions>();
            var mapper = new MarketDescriptionsMapperFactory();

            var dataProvider = new DataProvider<market_descriptions, IEnumerable<MarketDescriptionDTO>>(
                configInternal.ApiHost + "/v1/descriptions/{0}/markets.xml?include_mappings=true",
                dataFetcher,
                deserializer,
                mapper);

            var marketDescriptionCache = new MarketDescriptionCache(new MemoryCache("InvariantMarketDescriptionCache"),
                                                                 dataProvider,
                                                                 new [] {new CultureInfo("en")},
                                                                 configInternal.AccessToken,
                                                                 TimeSpan.FromHours(4),
                                                                 new CacheItemPolicy {SlidingExpiration = TimeSpan.FromDays(1)});
            var marketDescriptionProvider = new MarketDescriptionProvider(marketDescriptionCache, new[] {new CultureInfo("en")});
            return new SelectionBuilder(marketDescriptionProvider, configInternal);
        }
        #endregion

        /// <summary>
        /// Sets the Betradar event (match or outright) id
        /// </summary>
        /// <param name="eventId">The event identifier</param>
        /// <returns>Returns a <see cref="ISelectionBuilder" /></returns>
        public ISelectionBuilder SetEventId(long eventId)
        {
            _eventId = eventId.ToString();
            ValidateData(false, false, true);
            return this;
        }

        /// <summary>
        /// Sets the Betradar event (match or outright) id
        /// </summary>
        /// <param name="eventId">The event identifier</param>
        /// <returns>Returns a <see cref="ISelectionBuilder" /></returns>
        public ISelectionBuilder SetEventId(string eventId)
        {
            _eventId = eventId;
            ValidateData(false, false, true);
            return this;
        }

        /// <summary>
        /// Sets the selection id
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <returns>Returns a <see cref="ISelectionBuilder" /></returns>
        /// <value>Should be composed according to specification</value>
        public ISelectionBuilder SetId(string id)
        {
            _id = id;
            ValidateData(false, true);
            return this;
        }

        /// <summary>
        /// Sets the identifier lo
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="subType">Type of the sub</param>
        /// <param name="sov">The sov</param>
        /// <param name="selectionIds">The selection ids</param>
        /// <returns>ISelectionBuilder</returns>
        public ISelectionBuilder SetIdLo(int type, int subType, string sov, string selectionIds)
        {
            if (subType < 0)
            {
                subType = 0;
            }
            if (string.IsNullOrEmpty(sov))
            {
                sov = "*";
            }
            _id = $"live:{type}/{subType}/{sov}";
            if (!string.IsNullOrEmpty(selectionIds))
            {
                _id += "/" + selectionIds;
            }
            ValidateData(false, true);
            return this;
        }

        /// <summary>
        /// Sets the identifier lcoo
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="sportId">The sport identifier</param>
        /// <param name="sov">The sov</param>
        /// <param name="selectionIds">The selection ids</param>
        /// <returns>ISelectionBuilder</returns>
        public ISelectionBuilder SetIdLcoo(int type, int sportId, string sov, string selectionIds)
        {
            if (string.IsNullOrEmpty(sov))
            {
                sov = "*";
            }
            _id = $"lcoo:{type}/{sportId}/{sov}";
            if (!string.IsNullOrEmpty(selectionIds))
            {
                _id += "/" + selectionIds;
            }
            ValidateData(false, true);
            return this;
        }

        /// <summary>
        /// Sets the selection id for UOF
        /// </summary>
        /// <param name="product">The product to be used</param>
        /// <param name="sportId">The UF sport id</param>
        /// <param name="marketId">The UF market id</param>
        /// <param name="selectionId">The selection id</param>
        /// <param name="specifiers">The UF array of specifiers represented as string separated with '|'</param>
        /// <param name="sportEventStatus">The UF sport event status properties</param>
        /// <returns>Returns a <see cref="ISelectionBuilder" /></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <value>Should be composed according to specification</value>
        /// <example>
        /// SetIdUof(1, "sr:sport:1", 101, "10", "total=3.0|playerid=sr:player:10201");
        /// </example>
        public ISelectionBuilder SetIdUof(int product, string sportId, int marketId, string selectionId, string specifiers, IReadOnlyDictionary<string, object> sportEventStatus)
        {
            IDictionary<string, string> specs = null;
            if (!string.IsNullOrEmpty(specifiers))
            {
                specs = new Dictionary<string, string>();
                foreach (var spec in specifiers.Split('|'))
                {
                    var s = spec.Split('=');
                    Contract.Assume(s.Length == 2);
                    specs.Add(s[0], s[1]);
                }
            }
            return SetIdUof(product, sportId, marketId, selectionId, (IReadOnlyDictionary<string, string>) specs, sportEventStatus);
        }

        /// <summary>
        /// Sets the selection id for UOF
        /// </summary>
        /// <param name="product">The product to be used</param>
        /// <param name="sportId">The UF sport id</param>
        /// <param name="marketId">The UF market id</param>
        /// <param name="selectionId">The selection id</param>
        /// <param name="specifiers">The array of specifiers</param>
        /// <param name="sportEventStatus">The UF sport event status properties</param>
        /// <returns>Returns a <see cref="ISelectionBuilder" /></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <value>Should be composed according to specification</value>
        public ISelectionBuilder SetIdUof(int product, string sportId, int marketId, string selectionId, IReadOnlyDictionary<string, string> specifiers, IReadOnlyDictionary<string, object> sportEventStatus)
        {
            URN sportUrn;
            if (!URN.TryParse(sportId, out sportUrn))
            {
                throw new ArgumentException("SportId is not valid.");
            }
            if (!Enum.IsDefined(typeof(Product), product))
            {
                throw new ArgumentException("Product is not valid.");
            }

            _id = $"uof:{product}/{sportId}/{marketId}";
            if (!string.IsNullOrEmpty(selectionId))
            {
                _id += "/" + selectionId;
            }
            var newSpecifiers = HandleMarketDescription(product, sportId, marketId, specifiers, sportEventStatus);
            if (newSpecifiers != null && newSpecifiers.Any())
            {
                var specs = newSpecifiers.Aggregate(string.Empty, (s, pair) => s + "&" + pair.Key + "=" + pair.Value);
                Contract.Assume(specs != null && specs.Length > 1);
                _id += "?" + specs.Substring(1);
            }
            ValidateData(false, true);
            return this;
        }

        /// <summary>
        /// Sets the odds multiplied by 10000 and rounded to int value
        /// </summary>
        /// <param name="odds">The odds value to be set</param>
        /// <returns>Returns a <see cref="ISelectionBuilder" /></returns>
        public ISelectionBuilder SetOdds(int odds)
        {
            _odds = odds;
            ValidateData(false, false, false, true);
            return this;
        }

        /// <summary>
        /// Sets the <see cref="ISelection" /> properties
        /// </summary>
        /// <param name="eventId">The event id</param>
        /// <param name="id">The selection id</param>
        /// <param name="odds">The odds value to be set</param>
        /// <param name="isBanker"></param>
        /// <returns>Returns a <see cref="ISelectionBuilder" /></returns>
        public ISelectionBuilder Set(long eventId, string id, int odds, bool isBanker)
        {
            return Set(eventId.ToString(), id, odds, isBanker);
        }

        /// <summary>
        /// Sets the <see cref="ISelection" /> properties
        /// </summary>
        /// <param name="eventId">The event id</param>
        /// <param name="id">The selection id</param>
        /// <param name="odds">The odds value to be set</param>
        /// <param name="isBanker"></param>
        /// <returns>Returns a <see cref="ISelectionBuilder" /></returns>
        public ISelectionBuilder Set(string eventId, string id, int odds, bool isBanker)
        {
            Contract.Requires(odds >= 10000 && odds <= 1000000000);

            _eventId = eventId;
            _id = id;
            _odds = odds;
            ValidateData(true);
            return this;
        }

        /// <summary>
        /// Sets the banker property
        /// </summary>
        /// <param name="isBanker">if set to <c>true</c> [is banker]</param>
        /// <returns>Returns a <see cref="ISelectionBuilder" /></returns>
        public ISelectionBuilder SetBanker(bool isBanker)
        {
            _isBanker = isBanker;
            return this;
        }

        /// <summary>
        /// Builds the <see cref="ISelection" />
        /// </summary>
        /// <returns>Returns a <see cref="ITicketBuilder" /></returns>
        public ISelection Build()
        {
            ValidateData(true);
            return new Selection(_eventId, _id, _odds, _isBanker);
        }

        private void ValidateData(bool all = false, bool id = false, bool eventId = false, bool odds = false)
        {
            if (all || id)
            {
                if (string.IsNullOrEmpty(_id) || !TicketHelper.ValidStringId(_id, false, 1, 1000))
                {
                    throw new ArgumentException($"Id {_id} not valid.");
                }
            }
            if (all || eventId)
            {
                if (string.IsNullOrEmpty(_eventId) || !TicketHelper.ValidStringId(_eventId, false, 1, 100))
                {
                    throw new ArgumentException($"EventId {_eventId} not valid.");
                }
            }
            if (all || odds)
            {
                if (!(_odds >= 10000 && _odds <= 1000000000))
                {
                    throw new ArgumentException($"Odds {_odds} not valid.");
                }
            }
        }

        private IReadOnlyDictionary<string, string> HandleMarketDescription(int productId,
                                                                            string sportId,
                                                                            int marketId,
                                                                            IReadOnlyDictionary<string, string> specifiers,
                                                                            IReadOnlyDictionary<string, object> sportEventStatus)
        {
            Contract.Requires(marketId > 0);

            if (!_config.ProvideAdditionalMarketSpecifiers)
            {
                return specifiers;
            }

            MarketDescriptionCacheItem marketDescription = null;
            try
            {
                marketDescription = _marketDescriptionProvider.GetMarketDescriptorAsync(marketId, null).Result;
            }
            catch (Exception ex)
            {
                var baseEx = ex.GetBaseException();
                if (baseEx.InnerException != null)
                {
                    baseEx = baseEx.InnerException;
                    if (baseEx.InnerException != null)
                    {
                        baseEx = baseEx.InnerException;
                    }
                }
                if (baseEx is CacheItemNotFoundException)
                {
                    ExecutionLog.Warn($"No market description found for marketId={marketId}, sportId={sportId}, productId={productId}. Ex: {baseEx.Message}");
                }
                else if (baseEx is CommunicationException)
                {
                    ExecutionLog.Warn($"Exception during fetching market descriptions. Ex: {baseEx.Message}");
                }
                else
                {
                    ExecutionLog.Warn("Exception during fetching market descriptions.", baseEx);
                }
            }


            if (marketDescription == null)
            {
                ExecutionLog.Info($"No market description found for marketId={marketId}, sportId={sportId}, productId={productId}.");
                return specifiers;
            }

            //handle market 215
            if (marketId == 215)
            {
                var newSpecifiers = specifiers == null
                    ? new Dictionary<string, string>()
                    : specifiers as IDictionary<string, string>;

                newSpecifiers?.Add("$server", sportEventStatus["CurrentServer"].ToString());

                return newSpecifiers as IReadOnlyDictionary<string, string>;
            }

            //handle $score sov_template
            if (marketDescription.Mappings == null)
            {
                ExecutionLog.Info($"Market description {marketDescription.Id} has no mapping.");
                return specifiers;
            }
            var marketMapping = marketDescription.Mappings.FirstOrDefault(f => f.ProductId == productId && Equals(f.SportId, URN.Parse(sportId)));
            if (marketMapping == null || marketMapping.ProductId == 0)
            {
                ExecutionLog.Info($"Market description {marketDescription.Id} has no mapping sportId={sportId}, productId={productId}.");
                return specifiers;
            }

            if (!string.IsNullOrEmpty(marketMapping.SovTemplate) && marketMapping.SovTemplate.Contains("{$score}"))
            {
                if (sportEventStatus == null)
                {
                    throw new ArgumentException("SportEventStatus is missing.");
                }
                if (!sportEventStatus.ContainsKey("HomeScore"))
                {
                    throw new ArgumentException("SportEventStatus is missing HomeScore property.");
                }
                if (!sportEventStatus.ContainsKey("AwayScore"))
                {
                    throw new ArgumentException("SportEventStatus is missing AwayScore property.");
                }

                var newSpecifiers = specifiers == null
                        ? new Dictionary<string, string>()
                        : specifiers as IDictionary<string, string>;

                Debug.Assert(newSpecifiers != null, "newSpecifiers != null");
                newSpecifiers.Add("$score", $"{sportEventStatus["HomeScore"]}:{sportEventStatus["AwayScore"]}");

                return newSpecifiers as IReadOnlyDictionary<string, string>;
            }

            return specifiers;
        }
    }
}