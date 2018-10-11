/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Common;
using Sportradar.MTS.SDK.Entities.Contracts;
using Sportradar.MTS.SDK.Entities.Enums;

namespace Sportradar.MTS.SDK.Entities.Interfaces
{
    /// <summary>
    /// Defines a contract for ticket submission response
    /// </summary>
    [ContractClass(typeof(TicketCashoutResponseContract))]
    public interface ITicketCashoutResponse : ISdkTicket, IAcknowledgeable
    {
        /// <summary>
        /// Gets the status of the ticket cashout submission
        /// </summary>
        CashoutAcceptance Status { get; }

        /// <summary>
        /// Gets the response reason
        /// </summary>
        IResponseReason Reason { get; }

        /// <summary>
        /// Gets the response signature/hash (previous BetAcceptanceId)
        /// </summary>
        string Signature { get; }
    }
}