/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Sportradar.MTS.SDK.Entities.Internal.Cache;

namespace Sportradar.MTS.SDK.Entities.Contracts
{
    [ContractClassFor(typeof(IMarketDescriptionCache))]
    abstract class MarketDescriptionCacheContract : IMarketDescriptionCache
    {
        public Task<MarketDescriptionCacheItem> GetMarketDescriptorAsync(int marketId, string variant, IEnumerable<CultureInfo> cultures)
        {
            Contract.Requires(cultures != null && cultures.Any());
            Contract.Ensures(Contract.Result<Task<MarketDescriptionCacheItem>>() != null);

            return Contract.Result<Task<MarketDescriptionCacheItem>>();
        }
    }
}
