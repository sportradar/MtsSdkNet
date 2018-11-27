/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Enums;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Contracts
{
    [ContractClassFor(typeof(IBetBonus))]
    internal abstract class BetBonusContract : IBetBonus
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
    }
}

