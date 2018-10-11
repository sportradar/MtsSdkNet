/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Enums;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Contracts
{
    [ContractClassFor(typeof(IBetReoffer))]
    internal abstract class BetReofferContract : IBetReoffer
    {
        public long Stake
        {
            get
            {
                Contract.Ensures(Contract.Result<long>() > 0);
                Contract.Ensures(Contract.Result<long>() < 1000000000000000000);
                return Contract.Result<long>();
            }
        }

        public BetReofferType Type => Contract.Result<BetReofferType>();
    }
}

