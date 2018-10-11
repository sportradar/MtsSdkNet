/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Enums;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Internal.TicketImpl
{
    [Serializable]
    public class BetBonus : IBetBonus
    {
        public long Value { get; }
        public BetBonusType Type { get; }
        public BetBonusMode Mode { get; }

        public BetBonus(long value, BetBonusType betBonusType, BetBonusMode betBonusMode)
        {
            Contract.Requires(value > 0 && value < 1000000000000000000);

            Value = value;
            Type = betBonusType;
            Mode = betBonusMode;
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