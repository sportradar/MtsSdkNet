/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Contracts
{
    [ContractClassFor(typeof(ISelection))]
    internal abstract class SelectionContract : ISelection
    {
        [Pure]
        public string Id
        {
            get
            {
                Contract.Ensures(Contract.Result<string>().Length > 0 && Contract.Result<string>().Length <= 1000);
                return Contract.Result<string>();
            }
        }

        [Pure]
        public string EventId
        {
            get
            {
                Contract.Ensures(Contract.Result<string>().Length > 0 && Contract.Result<string>().Length <= 100);
                return Contract.Result<string>();
            }
        }

        [Pure]
        public int? Odds
        {
            get
            {
                Contract.Ensures(Contract.Result<int?>() == null || (Contract.Result<int?>() >= 10000 && Contract.Result<int?>() <= 1000000000));
                return Contract.Result<int?>();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is banker.
        /// </summary>
        /// <value><c>true</c> if this instance is banker; otherwise, <c>false</c></value>
        [Pure]
        public bool IsBanker => Contract.Result<bool>();
    }
}