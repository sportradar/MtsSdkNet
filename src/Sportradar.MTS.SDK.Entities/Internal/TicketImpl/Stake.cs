/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Enums;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Internal.TicketImpl
{
    public class Stake : IStake
    {
        public long Value { get; }
        public StakeType? Type { get; }

        public Stake(long value)
        {
            Contract.Requires(value > 0 && value < 1000000000000000000);

            Value = value;
        }

        public Stake(long value, StakeType type)
        {
            Contract.Requires(value > 0 && value < 1000000000000000000);

            Value = value;
            Type = type;
        }

        /// <summary>
        /// Defines invariant members of the class
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(Value > 0 && Value < 1000000000000000000);
        }
    }
}