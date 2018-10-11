/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Contracts;
using Sportradar.MTS.SDK.Entities.Enums;

namespace Sportradar.MTS.SDK.Entities.Interfaces
{
    /// <summary>
    /// Defines a contract for classes implementing the stake of the bet
    /// </summary>
    [ContractClass(typeof(StakeContract))]
    public interface IStake
    {
        /// <summary>
        /// Gets the value
        /// </summary>
        /// <value>Quantity multiplied by 10000 and rounded to a long value</value>
        long Value { get; }

        /// <summary>
        /// Gets the type of the stake
        /// </summary>
        /// <value>(optional, default total)</value>
        StakeType? Type { get; }
    }
}