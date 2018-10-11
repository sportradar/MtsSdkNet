/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Contracts;

namespace Sportradar.MTS.SDK.Entities.Interfaces
{
    /// <summary>
    /// Defines a contract for a ticket bet
    /// </summary>
    [ContractClass(typeof(BetContract))]
    public interface IBet
    {
        /// <summary>
        /// Gets the bonus of the bet (optional, default null)
        /// </summary>
        IBetBonus Bonus { get; }

        /// <summary>
        /// Gets the stake of the bet
        /// </summary>
        IStake Stake { get; }

        /// <summary>
        /// Gets the id of the bet
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets array of all the systems (optional, if missing then complete accumulator is used)
        /// </summary>
        IEnumerable<int> SelectedSystems { get; }

        /// <summary>
        /// Gets the array of selections which form the bet
        /// </summary>
        IEnumerable<ISelection> Selections { get; }

        /// <summary>
        /// Gets the reoffer reference bet id
        /// </summary>
        string ReofferRefId { get; }

        /// <summary>
        /// Gets the sum of all wins for all generated combinations for this bet (in ticket currency, used in validation)
        /// </summary>
        long SumOfWins { get; }
    }
}