/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Internal.REST.ClientApi;

namespace Sportradar.MTS.SDK.Entities.Internal.REST.Dto
{
    /// <summary>
    /// A data-transfer-object for max stake
    /// </summary>
    public class MaxStakeDTO
    {
        public long MaxStake { get; }

        internal MaxStakeDTO(MaxStakeResponse maxStakeResponse)
        {
            Contract.Requires(maxStakeResponse != null);
            Contract.Requires(maxStakeResponse.MaxStake != null);
            Contract.Requires(maxStakeResponse.MaxStake.HasValue);

            MaxStake = Convert.ToInt64(maxStakeResponse.MaxStake.Value);
        }
    }
}
