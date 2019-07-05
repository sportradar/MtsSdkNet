/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */

using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Internal.Dto.ClientApi;

namespace Sportradar.MTS.SDK.Entities.Internal.REST.ClientApiImpl
{
    /// <summary>
    /// A data-transfer-object for max stake
    /// </summary>
    public class MaxStakeImpl
    {
        public long MaxStake { get; }

        internal MaxStakeImpl(MaxStakeResponseDTO maxStakeResponseDto)
        {
            Contract.Requires(maxStakeResponseDto != null);

            MaxStake = maxStakeResponseDto.MaxStake;
        }
    }
}
