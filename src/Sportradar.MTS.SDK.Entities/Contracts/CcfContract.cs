/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Contracts
{
    [ContractClassFor(typeof(ICcf))]
    internal abstract class CcfContract : ICcf
    {
        public long Ccf => Contract.Result<long>();

        public IEnumerable<ISportCcf> SportCcfDetails
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<ISportCcf>>() != null);
                return Contract.Result<IEnumerable<ISportCcf>>();
            }
        }
    }
}

