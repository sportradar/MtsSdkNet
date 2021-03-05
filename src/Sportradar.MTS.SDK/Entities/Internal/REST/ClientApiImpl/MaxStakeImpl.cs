/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */

using Dawn;

namespace Sportradar.MTS.SDK.Entities.Internal.REST.ClientApiImpl
{
    /// <summary>
    /// A data-transfer-object for max stake
    /// </summary>
    internal class MaxStakeImpl
    {
        public long MaxStake { get; }

        internal MaxStakeImpl(Entities.Internal.Dto.ClientApi.MaxStakeResponseDTO maxStakeResponseDto)
        {
            Guard.Argument(maxStakeResponseDto, nameof(maxStakeResponseDto)).NotNull();

            MaxStake = maxStakeResponseDto.MaxStake;
        }
    }
}
