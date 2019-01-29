/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using log4net;
using Metrics;
using Sportradar.MTS.SDK.Common.Exceptions;
using Sportradar.MTS.SDK.Common.Log;

namespace Sportradar.MTS.SDK.Common.Internal.Rest
{
    /// <summary>
    /// A implementation of <see cref="IDataFetcher"/> and <see cref="IDataPoster"/> which uses the HTTP requests to fetch or post the requested data. All request are logged.
    /// </summary>
    /// <seealso cref="IDataFetcher" />
    /// <remarks>ALL, DEBUG, INFO, WARN, ERROR, FATAL, OFF - the levels are defined in order of increasing priority</remarks>
    public class LogHttpDataFetcher : HttpDataFetcher
    {
        private static readonly ILog RestLog = SdkLoggerFactory.GetLoggerForRestTraffic(typeof(LogHttpDataFetcher));

        private readonly ISequenceGenerator _sequenceGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogHttpDataFetcher"/> class
        /// </summary>
        /// <param name="client">A <see cref="HttpClient"/> used to invoke HTTP requests</param>
        /// <param name="accessToken">A token used when making the http requests</param>
        /// <param name="sequenceGenerator">A <see cref="ISequenceGenerator"/> used to identify requests</param>
        /// <param name="connectionFailureLimit">Indicates the limit of consecutive request failures, after which it goes in "blocking mode"</param>
        /// <param name="connectionFailureTimeout">indicates the timeout after which comes out of "blocking mode" (in seconds)</param>
        public LogHttpDataFetcher(HttpClient client, string accessToken, ISequenceGenerator sequenceGenerator, int connectionFailureLimit = 5, int connectionFailureTimeout = 15)
            : base(client, accessToken, connectionFailureLimit, connectionFailureTimeout)
        {
            Contract.Requires(sequenceGenerator != null);
            Contract.Requires(connectionFailureLimit >= 1);
            Contract.Requires(connectionFailureTimeout >= 1);

            _sequenceGenerator = sequenceGenerator;
        }

        /// <summary>
        /// Asynchronously gets a <see cref="Stream" /> containing data fetched from the provided <see cref="Uri" />
        /// </summary>
        /// <param name="uri">The <see cref="Uri" /> of the resource to be fetched</param>
        /// <returns>A <see cref="Task" /> which, when completed will return a <see cref="Stream" /> containing fetched data</returns>
        /// <exception cref="CommunicationException">Failed to execute http get</exception>
        public override async Task<Stream> GetDataAsync(Uri uri)
        {
            Metric.Context("FEED").Meter("LogHttpDataFetcher->GetDataAsync", Unit.Requests).Mark();

            var dataId = _sequenceGenerator.GetNext().ToString("D7"); // because request can take long time, there may be several request at the same time; Id to know what belongs together.
            var watch = new Stopwatch();

            RestLog.Info($"Id:{dataId} Fetching url: {uri.AbsoluteUri}");
            watch.Start();

            Stream responseStream;
            try
            {
                responseStream = await base.GetDataAsync(uri).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                watch.Stop();
                RestLog.Error($"Id:{dataId} Fetch error at {watch.ElapsedMilliseconds} ms.");
                if (ex.GetType() != typeof(ObjectDisposedException) && ex.GetType() != typeof(TaskCanceledException))
                {
                    RestLog.Error(ex);
                }
                throw;
            }

            watch.Stop();
            if (!RestLog.IsDebugEnabled)
            {
                return responseStream;
            }

            var responseContent = new StreamReader(responseStream).ReadToEnd();

            responseContent = responseContent.Replace("\n", string.Empty);
            //A nasty hack needed to fix the 'stage' in the namespace for the race_summary element
            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream);
            writer.Write(responseContent);
            writer.Flush();
            memoryStream.Position = 0;

            RestLog.Debug($"Id:{dataId} Fetching from {uri.AbsoluteUri} took {watch.ElapsedMilliseconds} ms. Data={responseContent}");

            return memoryStream;
        }

        /// <summary>
        /// Asynchronously gets a <see cref="Stream"/> containing data fetched from the provided <see cref="Uri"/>
        /// </summary>
        /// <param name="uri">The <see cref="Uri"/> of the resource to be fetched</param>
        /// <param name="content">A <see cref="HttpContent"/> to be posted to the specific <see cref="Uri"/></param>
        /// <returns>A <see cref="Task"/> which, when successfully completed will return a <see cref="HttpResponseMessage"/></returns>
        /// <exception cref="CommunicationException">Failed to execute http post</exception>
        public override async Task<HttpResponseMessage> PostDataAsync(Uri uri, HttpContent content = null)
        {
            Metric.Context("FEED").Meter("LogHttpDataFetcher->PostDataAsync", Unit.Requests).Mark();

            var dataId = _sequenceGenerator.GetNext().ToString("D7");

            RestLog.Info($"Id:{dataId} Posting to url: {uri.AbsoluteUri}");
            if (content != null)
            {
                var s = await content.ReadAsStringAsync().ConfigureAwait(false);
                RestLog.Info($"Id:{dataId} Content: {s}");
            }

            var watch = new Stopwatch();
            watch.Start();

            HttpResponseMessage response;
            try
            {
                response = await base.PostDataAsync(uri, content);
            }
            catch (Exception ex)
            {
                watch.Stop();
                if (!RestLog.IsInfoEnabled)
                {
                    RestLog.Error($"Id:{dataId} Error posting to url: {uri.AbsoluteUri}");
                }
                RestLog.Error($"Id:{dataId} Posting error at {watch.ElapsedMilliseconds} ms.");
                if (ex.GetType() != typeof(ObjectDisposedException) && ex.GetType() != typeof(TaskCanceledException))
                {
                    RestLog.Error(ex);
                }
                throw;
            }

            watch.Stop();
            RestLog.Debug($"Id:{dataId} Posting took {watch.ElapsedMilliseconds} ms. Response code: {(int)response.StatusCode}-{response.ReasonPhrase}.");
            return response;
        }
    }
}
