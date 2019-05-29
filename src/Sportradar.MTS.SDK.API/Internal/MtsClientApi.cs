/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Net.Http;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Metrics;
using Sportradar.MTS.SDK.Common.Log;
using Sportradar.MTS.SDK.Entities.Interfaces;
using Sportradar.MTS.SDK.Entities.Internal;
using Sportradar.MTS.SDK.Entities.Internal.REST.Dto;

namespace Sportradar.MTS.SDK.API
{
    /// <summary>
    /// A <see cref="IMtsClientApi"/> implementation acting as an entry point to the MTS Client API
    /// </summary>
    public class MtsClientApi : IMtsClientApi
    {
        /// <summary>
        /// A log4net.ILog instance used for logging execution logs
        /// </summary>
        private static readonly ILog ExecutionLog = SdkLoggerFactory.GetLoggerForRestTraffic(typeof(MtsClientApi));

        /// <summary>
        /// A log4net.ILog instance used for logging client iteration logs
        /// </summary>
        private static readonly ILog InteractionLog = SdkLoggerFactory.GetLoggerForClientInteraction(typeof(MtsClientApi));

        /// <summary>
        /// The <see cref="IDataProvider{MaxStakeDTO}>"/> for getting max stake
        /// </summary>
        private readonly IDataProvider<MaxStakeDTO> _maxStakeDataProvider;

        /// <summary>
        /// The <see cref="IDataProvider{CcfDTO}>"/> for getting ccf
        /// </summary>
        private readonly IDataProvider<CcfDTO> _ccfDataProvider;

        /// <summary>
        /// The <see cref="IDataProvider{KeycloakAuthorizationDTO}>"/> for getting authorization token
        /// </summary>
        private readonly IDataProvider<KeycloakAuthorizationDTO> _authorizationDataProvider;

        /// <summary>
        /// Username used for getting authorization token
        /// </summary>
        private readonly string _username;

        /// <summary>
        /// Password used for getting authorization token
        /// </summary>
        private readonly string _password;

        /// <summary>
        /// Secret used for getting authorization token
        /// </summary>
        private readonly string _secret;

        /// <summary>
        /// Cache for storing authorization tokens
        /// </summary>
        private readonly ObjectCache _tokenCache = new MemoryCache("tokenCache");

        /// <summary>
        /// Lock for synchronizing access to token cache
        /// </summary>
        private readonly SemaphoreSlim _tokenSemaphore = new SemaphoreSlim(1, 1);

        public MtsClientApi(IDataProvider<MaxStakeDTO> maxStakeDataProvider, IDataProvider<CcfDTO> ccfDataProvider, IDataProvider<KeycloakAuthorizationDTO> authorizationDataProvider, string username, string password, string secret)
        {
            Contract.Requires(maxStakeDataProvider != null);
            Contract.Requires(ccfDataProvider != null);
            Contract.Requires(authorizationDataProvider != null);

            _maxStakeDataProvider = maxStakeDataProvider;
            _ccfDataProvider = ccfDataProvider;
            _authorizationDataProvider = authorizationDataProvider;
            _username = username;
            _password = password;
            _secret = secret;
        }

        /// <summary>
        /// Defines invariant members of the class
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(InteractionLog != null);
            Contract.Invariant(ExecutionLog != null);
            Contract.Invariant(_maxStakeDataProvider != null);
            Contract.Invariant(_ccfDataProvider != null);
            Contract.Invariant(_authorizationDataProvider != null);
            Contract.Invariant(_tokenCache != null);
            Contract.Invariant(_tokenSemaphore != null);
        }

        public async Task<long> GetMaxStakeAsync(ITicket ticket)
        {
            return await GetMaxStakeAsync(ticket, _username, _password);
        }

        public async Task<long> GetMaxStakeAsync(ITicket ticket, string username, string password)
        {
            Metric.Context("MtsClientApi").Meter("GetMaxStakeAsync", Unit.Items).Mark();
            InteractionLog.Info($"Called GetMaxStakeAsync with ticketId={ticket.TicketId}.");

            try
            {
                var token = await GetToken(username, password);
                var content = new StringContent(ticket.ToJson(), Encoding.UTF8, "application/json");
                var maxStake = await _maxStakeDataProvider.GetDataAsync(token, content, new[] { "" });
                if (maxStake == null)
                    throw new Exception("Failed to get max stake.");
                return maxStake.MaxStake;
            }
            catch (Exception e)
            {
                ExecutionLog.Error(e.Message, e);
                ExecutionLog.Warn($"Getting max stake for ticketId={ticket.TicketId} failed.");
                throw;
            }
        }

        public async Task<ICcf> GetCcfAsync(string sourceId)
        {
            return await GetCcfAsync(sourceId, _username, _password);
        }

        public async Task<ICcf> GetCcfAsync(string sourceId, string username, string password)
        {
            Metric.Context("MtsClientApi").Meter("GetCcfAsync", Unit.Items).Mark();
            InteractionLog.Info($"Called GetCcfAsync with sourceId={sourceId}.");

            try
            {
                var token = await GetToken(username, password);
                return await _ccfDataProvider.GetDataAsync(token, new[] { sourceId });
            }
            catch (Exception e)
            {
                ExecutionLog.Error(e.Message, e);
                ExecutionLog.Warn($"Getting ccf for sourceId={sourceId} failed.");
                throw;
            }
        }

        private async Task<string> GetToken(string username, string password)
        {
            var cacheKey = $"{_secret}:{username}:{password}";
            var ci = _tokenCache.GetCacheItem(cacheKey);
            if (ci?.Value != null)
                return (string) ci.Value;

            try
            {
                await _tokenSemaphore.WaitAsync();
                ci = _tokenCache.GetCacheItem(cacheKey);
                if (ci?.Value != null)
                    return (string)ci.Value;

                var content = new FormUrlEncodedContent(new KeyValuePair<string, string>[]
                    {
                        new KeyValuePair<string, string>("client_id", "mts-edge-ext"),
                        new KeyValuePair<string, string>("client_secret", _secret),
                        new KeyValuePair<string, string>("username", username),
                        new KeyValuePair<string, string>("password", password),
                        new KeyValuePair<string, string>("grant_type", "password"),
                        new KeyValuePair<string, string>("client_auth_method", "client-secret")
                    });
                try
                {
                    var authorization = await _authorizationDataProvider.GetDataAsync(content, new[] { "" });
                    _tokenCache.Add(cacheKey, authorization.AccessToken, authorization.Expires.AddSeconds(-30));
                    return authorization.AccessToken;
                }
                catch (Exception e)
                {
                    ExecutionLog.Error(e.Message, e);
                    ExecutionLog.Warn("Error getting token from auth server.");
                    throw;
                }
            }
            finally
            {
                _tokenSemaphore.Release();
            }
        }
    }
}
