/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Interfaces;

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
                Contract.Ensures(Contract.Result<string>() == null || Contract.Result<string>().Length > 0);
                return Contract.Result<string>();
            }
        }

        /// <summary>
        /// Gets the cancel percent of the assigned bet
        /// </summary>
        /// <value>The cancel percent</value>
        public long CancelPercent
        {
            get
            {
                Contract.Ensures(Contract.Result<long>() > 0);
                return Contract.Result<long>();
            }
        }
    }
}

