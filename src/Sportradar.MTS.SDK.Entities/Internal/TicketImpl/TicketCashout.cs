/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Internal.TicketImpl
{
    /// <summary>
    /// Implementation of <see cref="ITicketCashout"/>
    /// </summary>
    /// <seealso cref="ITicketCashout" />
    [Serializable]
    public class TicketCashout : ITicketCashout
    {
        /// <summary>
        /// Gets the timestamp of ticket placement (UTC)
        /// </summary>
        /// <value>The timestamp</value>
        public DateTime Timestamp { get; }
        /// <summary>
        /// Gets the ticket id
        /// </summary>
        /// <value>Unique ticket id (in the client's system)</value>
        public string TicketId { get; }
        /// <summary>
        /// Get the bookmaker id (client's id provided by Sportradar)
        /// </summary>
        /// <value>The bookmaker identifier</value>
        public int BookmakerId { get; }
        /// <summary>
        /// Gets the cashout stake
        /// </summary>
        /// <value>The cashout stake</value>
        public long? CashoutStake { get; }
        /// <summary>
        /// Gets the cashout percent
        /// </summary>
        /// <value>The cashout percent</value>
        public int? CashoutPercent { get; }
        /// <summary>
        /// Gets the list of <see cref="IBetCashout"/>
        /// </summary>
        /// <value>The list of <see cref="IBetCashout"/></value>
        public IEnumerable<IBetCashout> BetCashouts { get; }
        /// <summary>
        /// Gets the ticket format version
        /// </summary>
        /// <value>The version</value>
        public string Version { get; }
        /// <summary>
        /// Gets the correlation identifier
        /// </summary>
        /// <value>The correlation identifier</value>
        /// <remarks>Only used to relate ticket with its response</remarks>
        public string CorrelationId { get; }

        public string ToJson()
        {
            var dto = EntitiesMapper.Map(this);
            return dto.ToJson();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketCashout"/> class
        /// </summary>
        /// <param name="ticketId">The ticket identifier</param>
        /// <param name="bookmakerId">The bookmaker identifier</param>
        /// <param name="stake">The cashout stake</param>
        /// <param name="percent">The cashout percent</param>
        /// <param name="betCashouts">The list of <see cref="IBetCashout"/></param>
        public TicketCashout(string ticketId, int bookmakerId, long? stake, int? percent, IReadOnlyCollection<IBetCashout> betCashouts)
        {
            Contract.Requires(TicketHelper.ValidateTicketId(ticketId));
            Contract.Requires(bookmakerId > 0);
            Contract.Requires(stake > 0 || percent > 0 || (betCashouts != null && betCashouts.Any()));

            if (percent != null && stake == null)
            {
                throw new ArgumentException("If percent is set, also stake must be.");
            }

            if (betCashouts != null && (stake != null || percent != null))
            {
                throw new ArgumentException("Stake and/or Percent cannot be set at the same time as BetCashouts.");
            }

            TicketId = ticketId;
            BookmakerId = bookmakerId;
            CashoutStake = stake;
            CashoutPercent = percent;
            BetCashouts = betCashouts;
            Timestamp = DateTime.UtcNow;
            Version = TicketHelper.MtsTicketVersion;
            CorrelationId = TicketHelper.GenerateTicketCorrelationId();
        }

        /// <summary>
        /// Defines invariant members of the class
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(TicketHelper.ValidateTicketId(TicketId));
            Contract.Invariant(!string.IsNullOrEmpty(Version));
            Contract.Invariant(Timestamp > DateTime.MinValue);
            Contract.Invariant(BookmakerId > 0);
            Contract.Invariant(CashoutStake > 0 || CashoutPercent > 0 || (BetCashouts != null && BetCashouts.Any()));
        }
    }
}