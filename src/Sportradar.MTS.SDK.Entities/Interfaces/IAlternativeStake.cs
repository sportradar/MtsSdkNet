/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Contracts;

namespace Sportradar.MTS.SDK.Entities.Interfaces
{
    /// <summary>
    /// Defines a contract for alternative stake, mutually exclusive with reoffer
    /// </summary>
    [ContractClass(typeof(AlternativeStakeContract))]
    public interface IAlternativeStake
    {
        /// <summary>
        /// Gets the stake
        /// </summary>
        long Stake { get; }
    }
}