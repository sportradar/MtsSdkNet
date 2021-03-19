/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using Dawn;
using log4net;
using Metrics;
using Sportradar.MTS.SDK.Common.Internal;
using Sportradar.MTS.SDK.Common.Log;
using Sportradar.MTS.SDK.Entities.Internal;
using Sportradar.MTS.SDK.Entities.Internal.REST.ClientApiImpl;

namespace Sportradar.MTS.SDK.API.Internal.MtsAuth
{
    internal class MtsAuthService : IMtsAuthService
    {
        /// <summary>
        /// A logger instance used for logging execution logs
        /// </summary>
        private static readonly ILog ExecutionLog = SdkLoggerFactory.GetLoggerForExecution(typeof(MtsAuthService));

        /// <summary>
        /// A logger instance used for logging client iteration logs
        /// </summary>
        private static readonly ILog InteractionLog = SdkLoggerFactory.GetLoggerForClientInteraction(typeof(MtsAuthService));

        /// <summary>
        /// The <see cref="IDataProvider{T}"/> for getting authorization token
        /// </summary>
        private readonly IDataProvider<KeycloakAuthorization> _authorizationDataProvider;

        /// <summary>
        /// Username used for getting authorization token
        /// </summary>
        private readonly string _keycloackUsername;

        /// <summary>
        /// Password used for getting authorization token
        /// </summary>
        private readonly string _keycloackPassword;

        /// <summary>
        /// Secret used for getting authorization token
        /// </summary>
        private readonly string _keycloackSecret;

        /// <summary>
        /// Cache for storing authorization tokens
        /// </summary>
        private readonly ObjectCache _tokenCache = new MemoryCache("tokenCache");

        /// <summary>
        /// Lock for synchronizing access to token cache
        /// </summary>
        private readonly SemaphoreSlim _tokenSemaphore = new SemaphoreSlim(1, 1);

        public MtsAuthService(IDataProvider<KeycloakAuthorization> authorizationDataProvider, 
                              string keycloackUsername, 
                              string keycloackPassword, 
                              string keycloackSecret)
        {
            Guard.Argument(authorizationDataProvider, nameof(authorizationDataProvider)).NotNull();
            
            _authorizationDataProvider = authorizationDataProvider;
            _keycloackUsername = keycloackUsername;
            _keycloackPassword = keycloackPassword;
            _keycloackSecret = keycloackSecret;

            if (string.IsNullOrEmpty(_keycloackSecret))
            {
                ExecutionLog.Debug("No keycloack secret set.");
            }
        }

        /// <inheritdoc />
        public async Task<string> GetTokenAsync(string keycloackUsername = null, string keycloackPassword = null)
        {
            if(string.IsNullOrEmpty(keycloackUsername) && !string.IsNullOrEmpty(_keycloackUsername))
            {
                keycloackUsername = _keycloackUsername;
            }
            if(string.IsNullOrEmpty(keycloackPassword) && !string.IsNullOrEmpty(_keycloackPassword))
            {
                keycloackPassword = _keycloackPassword;
            }

            if (string.IsNullOrEmpty(_keycloackSecret))
            {
                throw new InvalidOperationException("Missing keycloack secret. Must to be set via configuration.");
            }
            if (string.IsNullOrEmpty(keycloackUsername))
            {
                throw new ArgumentNullException(nameof(keycloackUsername), "Missing keycloack username. Must be provided or set via configuration.");
            }
            if (string.IsNullOrEmpty(keycloackPassword))
            {
                throw new ArgumentNullException(nameof(keycloackPassword), "Missing keycloack password. Must be provided or set via configuration.");
            }

            var result = await GetTokenAsync(_keycloackSecret, keycloackUsername, keycloackPassword).ConfigureAwait(false);
            return result;
        }

        private async Task<string> GetTokenAsync(string keycloackSecret, string keycloackUsername, string keycloackPassword)
        {
            InteractionLog.Info("Getting access token");
            Metric.Context("MtsAuthService").Meter("GetTokenAsync", Unit.Calls).Mark();

            var cacheKey = $"{keycloackSecret}:{keycloackUsername}:{keycloackPassword}";
            var ci = _tokenCache.GetCacheItem(cacheKey);
            if (ci?.Value != null)
            {
                InteractionLog.Info("Retrieved access token from cache");
                return (string) ci.Value;
            }

            try
            {
                await _tokenSemaphore.WaitAsync();
                ci = _tokenCache.GetCacheItem(cacheKey);
                if (ci?.Value != null)
                {
                    InteractionLog.Info("Retrieved access token from cache");
                    return (string)ci.Value;
                }

                InteractionLog.Info($"Requesting access token for secret={SdkInfo.Obfuscate(keycloackSecret, true)}, username={keycloackUsername}, password={SdkInfo.Obfuscate(keycloackPassword)}");
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("client_id", "mts-edge-ext"),
                    new KeyValuePair<string, string>("client_secret", _keycloackSecret),
                    new KeyValuePair<string, string>("username", keycloackUsername),
                    new KeyValuePair<string, string>("password", keycloackPassword),
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("client_auth_method", "client-secret")
                });
                try
                {
                    var authorization = await _authorizationDataProvider.PostDataAsync(content, new[] { "" }).ConfigureAwait(false);
                    _tokenCache.Add(cacheKey, authorization.AccessToken, authorization.Expires.AddSeconds(-30));
                    InteractionLog.Info("Retrieved access token from server");
                    return authorization.AccessToken;
                }
                catch (Exception e)
                {
                    ExecutionLog.Error(e.Message, e);
                    ExecutionLog.Warn("Error getting token from authorization server.");
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