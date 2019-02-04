/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */

using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Contracts
{
    [ContractClassFor(typeof(ISportCcf))]
    internal abstract class SportCcfContract : ISportCcf
    {
        public string SportId
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return Contract.Result<string>();
            }
        }

        public long PrematchCcf => Contract.Result<long>();

        public long LiveCcf => Contract.Result<long>();
    }
}

