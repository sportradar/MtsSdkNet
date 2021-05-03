/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dawn;
using log4net;
using Metrics;
using Sportradar.MTS.SDK.API.Internal.MtsAuth;
using Sportradar.MTS.SDK.Common.Exceptions;
using Sportradar.MTS.SDK.Common.Log;
using Sportradar.MTS.SDK.Entities.Interfaces;
using Sportradar.MTS.SDK.Entities.Internal;
using Sportradar.MTS.SDK.Entities.Internal.REST.ClientApiImpl;

namespace Sportradar.MTS.SDK.API.Internal
{
    /// <summary>
    /// A <see cref="IMtsClientApi"/> implementation acting as an entry point to the MTS Client API
    /// </summary>
    internal class MtsClientApi : IMtsClientApi
    {
        /// <summary>
        /// A log4net.ILog instance used for logging execution logs
        /// </summary>
        private static readonly ILog ExecutionLog = SdkLoggerFactory.GetLoggerForExecution(typeof(MtsClientApi));

        /// <summary>
        /// A log4net.ILog instance used for logging client iteration logs
        /// </summary>
        private static readonly ILog InteractionLog = SdkLoggerFactory.GetLoggerForClientInteraction(typeof(MtsClientApi));

        /// <summary>
        /// The <see cref="IDataProvider{MaxStakeImpl}"/> for getting max stake
        /// </summary>
        private readonly IDataProvider<MaxStakeImpl> _maxStakeDataProvider;

        /// <summary>
        /// The <see cref="IDataProvider{CcfImpl}"/> for getting ccf
        /// </summary>
        private readonly IDataProvider<CcfImpl> _ccfDataProvider;
        
        /// <summary>
        /// The MTS authentication service
        /// </summary>
        private readonly IMtsAuthService _mtsAuthService;

        public MtsClientApi(IDataProvider<MaxStakeImpl> maxStakeDataProvider, IDataProvider<CcfImpl> ccfDataProvider, IMtsAuthService mtsAuthService)
        {
            Guard.Argument(maxStakeDataProvider, nameof(maxStakeDataProvider)).NotNull();
            Guard.Argument(ccfDataProvider, nameof(ccfDataProvider)).NotNull();
            Guard.Argument(mtsAuthService, nameof(mtsAuthService)).NotNull();

            _maxStakeDataProvider = maxStakeDataProvider;
            _ccfDataProvider = ccfDataProvider;
            _mtsAuthService = mtsAuthService;
        }

        public async Task<long> GetMaxStakeAsync(ITicket ticket)
        {
            Guard.Argument(ticket, nameof(ticket)).NotNull();

            return await GetMaxStakeAsync(ticket, null, null).ConfigureAwait(false);
        }

        public async Task<long> GetMaxStakeAsync(ITicket ticket, string username, string password)
        {
            Guard.Argument(ticket, nameof(ticket)).NotNull();

            Metric.Context("MtsClientApi").Meter("GetMaxStakeAsync", Unit.Calls).Mark();
            InteractionLog.Info($"Called GetMaxStakeAsync with ticketId={ticket.TicketId}.");

            try
            {
                var token = await _mtsAuthService.GetTokenAsync(username, password).ConfigureAwait(false);
                var content = new StringContent(ticket.ToJson(), Encoding.UTF8, "application/json");
                var maxStake = await _maxStakeDataProvider.PostDataAsync(token, content, new[] { "" }).ConfigureAwait(false);
                if (maxStake == null)
                {
                    throw new MtsApiException("Failed to get max stake.", null);
                }
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
            Guard.Argument(sourceId, nameof(sourceId)).NotNull();

            return await GetCcfAsync(sourceId, null, null).ConfigureAwait(false);
        }

        public async Task<ICcf> GetCcfAsync(string sourceId, string username, string password)
        {
            Guard.Argument(sourceId, nameof(sourceId)).NotNull();
            Guard.Argument(username, nameof(username)).NotNull().NotEmpty();
            Guard.Argument(password, nameof(password)).NotNull().NotEmpty();

            Metric.Context("MtsClientApi").Meter("GetCcfAsync", Unit.Calls).Mark();
            InteractionLog.Info($"Called GetCcfAsync with sourceId={sourceId}.");

            try
            {
                var token = await _mtsAuthService.GetTokenAsync(username, password).ConfigureAwait(false);
                return await _ccfDataProvider.GetDataAsync(token, new[] { sourceId }).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                ExecutionLog.Error(e.Message, e);
                ExecutionLog.Warn($"Getting ccf for sourceId={sourceId} failed.");
                throw;
            }
        }
    }
}
