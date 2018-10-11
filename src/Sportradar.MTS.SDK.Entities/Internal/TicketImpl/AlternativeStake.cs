/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Internal.TicketImpl
{
    /// <summary>
    /// Implementation of <see cref="IAlternativeStake"/>
    /// </summary>
    /// <seealso cref="IAlternativeStake" />
    public class AlternativeStake : IAlternativeStake
    {
        /// <summary>
        /// Gets the stake
        /// </summary>
        /// <value>The stake</value>
        public long Stake { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlternativeStake"/> class
        /// </summary>
        /// <param name="stake">The stake</param>
        public AlternativeStake(long stake)
        {
            Contract.Requires(stake > 0 && stake < 1000000000000000000);

            Stake = stake;
        }

        /// <summary>
        /// Defines invariant members of the class
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(Stake > 0 && Stake < 1000000000000000000);
        }
    }
}