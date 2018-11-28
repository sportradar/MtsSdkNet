/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */

using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Contracts;

namespace Sportradar.MTS.SDK.Entities.Interfaces
{
    /// <summary>
    /// Defines a contract for auto accepted odds
    /// </summary>
    [ContractClass(typeof(AutoAcceptedOddsContract))]
    public interface IAutoAcceptedOdds
    {
        /// <summary>
        /// Selection index from 'ticket.selections' array (zero based)
        /// </summary>
        /// <returns>Selection index from 'ticket.selections' array (zero based)</returns>
        int SelectionIndex { get; }

        /// <summary>
        /// Odds with which the ticket was placed
        /// </summary>
        /// <returns>Odds with which the ticket was placed</returns>
        int RequestedOdds { get; }

        /// <summary>
        /// Odds with which the ticket was accepted
        /// </summary>
        /// <returns>Odds with which the ticket was accepted</returns>
        int UsedOdds { get; }
    }

}
