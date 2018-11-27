﻿/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */

using System;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Internal.TicketImpl
{
    public class BetCashout : IBetCashout
    {
        /// <summary>
        /// Gets the id of the bet
        /// </summary>
        public string BetId { get; }

        /// <summary>
        /// Gets the cashout stake of the assigned bet
        /// </summary>
        /// <value>The cashout stake</value>
        public long? CashoutStake { get; }

        /// <summary>
        /// Gets the cashout percent of the assigned bet
        /// </summary>
        /// <value>The cashout percent</value>
        public long? CashoutPercent { get; }

        /// <summary>
        /// Construct the bet cashout
        /// </summary>
        /// <param name="betId">The bet id</param>
        /// <param name="stake">The cashout stake value of the assigned bet (quantity multiplied by 10_000 and rounded to a long value)</param>
        /// <param name="percent">The cashout percent value of the assigned bet (quantity multiplied by 10_000 and rounded to a long value)</param>
        public BetCashout(string betId, long? stake, long? percent)
        {
            if (stake == null && percent == null)
            {
                throw new ArgumentException("Stake or percent must be set.");
            }
            if (stake != null && percent != null)
            {
                throw new ArgumentException("Stake and percent cannot be set at the same time.");
            }
            if (stake != null && stake < 1)
            {
                throw new ArgumentException("Stake not valid.");
            }
            if (percent != null && percent < 1)
            {
                throw new ArgumentException("Percent not valid.");
            }

            BetId = betId;
            CashoutStake = stake;
            CashoutPercent = percent;
        }
    }
}
