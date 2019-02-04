/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */

using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.API.Contracts
{
    [ContractClassFor(typeof(IMtsClientApi))]
    internal abstract class MtsClientApiContract : IMtsClientApi
    {
        public Task<long?> GetMaxStakeAsync(ITicket ticket)
        {
            Contract.Requires(ticket != null);
            return Contract.Result<Task<long?>>();
        }

        public Task<long?> GetMaxStakeAsync(ITicket ticket, string username, string password)
        {
            Contract.Requires(ticket != null);
            Contract.Requires(username != null);
            Contract.Requires(password != null);
            return Contract.Result<Task<long?>>();
        }

        public Task<ICcf> GetCcfAsync(string sourceId)
        {
            Contract.Requires(sourceId != null);
            return Contract.Result<Task<ICcf>>();
        }

        public Task<ICcf> GetCcfAsync(string sourceId, string username, string password)
        {
            Contract.Requires(sourceId != null);
            Contract.Requires(username != null);
            Contract.Requires(password != null);
            return Contract.Result<Task<ICcf>>();
        }
    }
}

