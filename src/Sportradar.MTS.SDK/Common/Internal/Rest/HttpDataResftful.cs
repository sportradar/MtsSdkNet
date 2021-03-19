﻿/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Dawn;
using log4net;
using Sportradar.MTS.SDK.Common.Exceptions;
using Sportradar.MTS.SDK.Common.Internal.Log;
using Sportradar.MTS.SDK.Common.Log;

namespace Sportradar.MTS.SDK.Common.Internal.Rest
{
    /// <summary>
    /// A <see cref="IDataRestful"/> which uses the HTTP requests to post/get/put/patch and delete the data
    /// </summary>
    [Log(LoggerType.RestTraffic)]
    internal class HttpDataRestful : HttpDataFetcher, IDataRestful
    {
        private static readonly ILog RestLog = SdkLoggerFactory.GetLoggerForRestTraffic(typeof(HttpDataRestful));

        /// <summary>
        /// A <see cref="HttpClient"/> used to invoke HTTP requests
        /// </summary>
        private readonly HttpClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpDataRestful"/> class
        /// </summary>
        /// <param name="client">A <see cref="T:System.Net.Http.HttpClient" /> used to invoke HTTP requests</param>
        /// <param name="accessToken">A token used when making the http requests</param>
        /// <param name="connectionFailureLimit">Indicates the limit of consecutive request failures, after which it goes in "blocking mode"</param>
        /// <param name="connectionFailureTimeout">indicates the timeout after which comes out of "blocking mode" (in seconds)</param>
        public HttpDataRestful(HttpClient client, string accessToken, int connectionFailureLimit = 5, int connectionFailureTimeout = 15)
            : base(client, accessToken, connectionFailureLimit, connectionFailureTimeout)
        {
            Guard.Argument(client, nameof(client)).NotNull();
            Guard.Argument(client.DefaultRequestHeaders, nameof(client.DefaultRequestHeaders)).NotNull();
            //Guard.Argument(!string.IsNullOrWhiteSpace(accessToken));
            Guard.Argument(connectionFailureLimit, nameof(connectionFailureLimit)).Positive();
            Guard.Argument(connectionFailureTimeout, nameof(connectionFailureTimeout)).Positive();

            _client = client;
            if (!string.IsNullOrEmpty(accessToken) && _client.DefaultRequestHeaders != null && !_client.DefaultRequestHeaders.Contains("x-access-token"))
            {
                _client.DefaultRequestHeaders.Add("x-access-token", accessToken);
            }
        }

        /// <summary>
        /// Asynchronously gets a <see cref="HttpResponseMessage"/> as a result of PUT request send to the provided <see cref="Uri"/>
        /// </summary>
        /// <param name="uri">The <see cref="Uri"/> of the resource to be send to</param>
        /// <param name="content">A <see cref="HttpContent"/> to be posted to the specific <see cref="Uri"/></param>
        /// <returns>A <see cref="Task"/> which, when completed will return a <see cref="HttpResponseMessage"/> containing status code and data</returns>
        /// <exception cref="CommunicationException">Failed to execute http post</exception>
        public virtual async Task<HttpResponseMessage> PutDataAsync(Uri uri, HttpContent content = null)
        {
            ValidateConnection(uri);
            var responseMessage = new HttpResponseMessage();
            try
            {
                RestLog.Info($"PutDataAsync url: {uri.AbsoluteUri}");
                responseMessage = await _client.PutAsync(uri, content ?? new StringContent(string.Empty));
                RecordSuccess();
                return responseMessage;
            }
            catch (Exception ex)
            {
                if (ex is HttpRequestException)
                {
                    RecordFailure();
                    throw new CommunicationException("Failed to execute http PUT request", uri.ToString(), responseMessage.StatusCode, ex);
                }
                throw;
            }
        }

        /// <summary>
        /// Asynchronously gets a <see cref="HttpResponseMessage"/> as a result of DELETE request send to the provided <see cref="Uri"/>
        /// </summary>
        /// <param name="uri">The <see cref="Uri"/> of the resource to be send to</param>
        /// <returns>A <see cref="Task"/> which, when completed will return a <see cref="HttpResponseMessage"/> containing status code and data</returns>
        /// <exception cref="CommunicationException">Failed to execute http post</exception>
        public virtual async Task<HttpResponseMessage> DeleteDataAsync(Uri uri)
        {
            ValidateConnection(uri);
            var responseMessage = new HttpResponseMessage();
            try
            {
                RestLog.Info($"DeleteDataAsync url: {uri.AbsoluteUri}");
                responseMessage = await _client.DeleteAsync(uri);
                RecordSuccess();
                return responseMessage;
            }
            catch (Exception ex)
            {
                if (ex is HttpRequestException)
                {
                    RecordFailure();
                    throw new CommunicationException("Failed to execute http DELETE request", uri.ToString(), responseMessage.StatusCode, ex);
                }
                throw;
            }
        }
    }
}
