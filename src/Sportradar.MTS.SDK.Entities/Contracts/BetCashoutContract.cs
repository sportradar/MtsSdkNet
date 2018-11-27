/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Enums;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Contracts
{
    [ContractClassFor(typeof(IBetCashout))]
    internal abstract class BetCashoutContract : IBetCashout
    {
        public long Value
        {
            get
            {
                Contract.Ensures(Contract.Result<long>() >= 0);
                Contract.Ensures(Contract.Result<long>() < 1000000000000000000);
                return Contract.Result<long>();
            }
        }

        public BetBonusType Type => Contract.Result<BetBonusType>();

        public BetBonusMode Mode => Contract.Result<BetBonusMode>();

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
        /// Gets the cashout stake of the assigned bet
        /// </summary>
        /// <value>The cashout stake</value>
        public long? CashoutStake
        {
            get
            {
                Contract.Ensures(Contract.Result<long?>() == null || Contract.Result<long?>() > 0);
                return Contract.Result<long?>();
            }
        }

        /// <summary>
        /// Gets the cashout percent of the assigned bet
        /// </summary>
        /// <value>The cashout percent</value>
        public long? CashoutPercent
        {
            get
            {
                Contract.Ensures(Contract.Result<long?>() == null || Contract.Result<long?>() > 0);
                return Contract.Result<long?>();
            }
        }
    }
}

