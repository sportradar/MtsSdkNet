/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Contracts
{
    [ContractClassFor(typeof(IAutoAcceptedOdds))]
    internal abstract class AutoAcceptedOddsContract : IAutoAcceptedOdds
    {
        /// <summary>
        /// Selection index from 'ticket.selections' array (zero based)
        /// </summary>
        /// <returns>Selection index from 'ticket.selections' array (zero based)</returns>
        public int SelectionIndex
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);
                Contract.Ensures(Contract.Result<int>() <= 62);
                return Contract.Result<int>();
            }
        }

        /// <summary>
        /// Odds with which the ticket was placed
        /// </summary>
        /// <returns>Odds with which the ticket was placed</returns>
        public int RequestedOdds
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 10000);
                Contract.Ensures(Contract.Result<int>() <= 1000000000);
                return Contract.Result<int>();
            }
        }

        /// <summary>
        /// Odds with which the ticket was accepted
        /// </summary>
        /// <returns>Odds with which the ticket was accepted</returns>
        public int UsedOdds
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 10000);
                Contract.Ensures(Contract.Result<int>() <= 1000000000);
                return Contract.Result<int>();
            }
        }
    }
}

