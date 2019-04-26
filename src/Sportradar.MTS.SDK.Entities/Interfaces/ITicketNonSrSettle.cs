/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */

using Sportradar.MTS.SDK.Entities.Contracts;
using System.Diagnostics.Contracts;

namespace Sportradar.MTS.SDK.Entities.Interfaces
{
    /// <summary>
    /// Contract defining Ticket that can be send to the MTS
    /// </summary>
    [ContractClass(typeof(TicketNonSrSettleContract))]
    public interface ITicketNonSrSettle : ISdkTicket
    {
        /// <summary>
        /// Get the bookmaker id (client's id provided by Sportradar)
        /// </summary>
        int BookmakerId { get; }
        /// <summary>
        /// Gets the non-sportradar settle stake
        /// </summary>
        long NonSrSettleStake { get; }
    }
}
