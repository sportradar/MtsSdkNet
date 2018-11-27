/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Interfaces;
using Sportradar.MTS.SDK.Entities.Internal;

namespace Sportradar.MTS.SDK.Entities.Contracts
{
    [ContractClassFor(typeof(IBetCancel))]
    internal abstract class BetCancelContract : IBetCancel
    {
        /// <summary>
        /// Gets the id of the bet
        /// </summary>
        public string BetId
        {
            get
            {
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()) && Contract.Result<string>().Length > 0);
                return Contract.Result<string>();
            }
        }

        /// <summary>
        /// Gets the cancel percent of the assigned bet
        /// </summary>
        /// <value>The cancel percent</value>
        public int? CancelPercent
        {
            get
            {
                Contract.Ensures(TicketHelper.ValidatePercent(Contract.Result<int?>()));
                return Contract.Result<int?>();
            }
        }
    }
}

