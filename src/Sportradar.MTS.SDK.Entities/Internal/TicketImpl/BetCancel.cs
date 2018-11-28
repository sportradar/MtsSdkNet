/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */

using System;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Internal.TicketImpl
{
    public class BetCancel : IBetCancel
    {
        /// <summary>
        /// Gets the id of the bet
        /// </summary>
        public string BetId { get; }

        /// <summary>
        /// Gets the cancel percent of the assigned bet
        /// </summary>
        /// <value>The cancel percent</value>
        public int? CancelPercent { get; }

        /// <summary>
        /// Construct the bet cashout
        /// </summary>
        /// <param name="betId">The bet id</param>
        /// <param name="percent">The cashout percent value of the assigned bet (quantity multiplied by 10_000 and rounded to a int value)</param>
        public BetCancel(string betId, int? percent)
        {
            if (!TicketHelper.ValidateTicketId(betId))
            {
                throw new ArgumentException("BetId not valid.");
            }
            if (!TicketHelper.ValidatePercent(percent))
            {
                throw new ArgumentException("Percent not valid.");
            }

            BetId = betId;
            CancelPercent = percent;
        }
    }
}
