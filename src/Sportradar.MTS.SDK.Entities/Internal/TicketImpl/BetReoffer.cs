/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Enums;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Internal.TicketImpl
{
    public class BetReoffer : IBetReoffer
    {
        public BetReofferType Type { get; }

        public long Stake { get; }

        public BetReoffer(long stake, BetReofferType type)
        {
            Contract.Requires(stake > 0 && stake < 1000000000000000000);

            Stake = stake;
            Type = type;
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