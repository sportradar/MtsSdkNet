/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Contracts;

namespace Sportradar.MTS.SDK.Entities.Interfaces
{
    /// <summary>
    /// Contract defining Ticket that can be send to the MTS
    /// </summary>
    [ContractClass(typeof(TicketReofferCancelContract))]
    public interface ITicketReofferCancel : ISdkTicket
    {
        /// <summary>
        /// Get the bookmaker id (client's id provided by Sportradar)
        /// </summary>
        int BookmakerId { get; }
    }
}