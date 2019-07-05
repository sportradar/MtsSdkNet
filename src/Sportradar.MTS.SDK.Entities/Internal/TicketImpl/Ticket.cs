﻿/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Newtonsoft.Json;
using Sportradar.MTS.SDK.Entities.Enums;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Internal.TicketImpl
{
    /// <summary>
    /// Implementation of <see cref="ITicket" /></summary>
    /// <seealso cref="ITicket" />
    [Serializable]
    public class Ticket : ITicket
    {
        /// <summary>
        /// Gets the ticket id
        /// </summary>
        /// <value>Unique ticket id (in the client's system)</value>
        public string TicketId { get; }
        /// <summary>
        /// Gets the collection of all bets
        /// </summary>
        /// <value>The bets</value>
        public IEnumerable<IBet> Bets { get; }
        /// <summary>
        /// Gets all of the ticket selections
        /// </summary>
        /// <value>Order is very important as they can be referenced by index</value>
        public IEnumerable<ISelection> Selections { get; }

        /// <summary>
        /// Gets the identification and settings of the ticket sender
        /// </summary>
        /// <value>The sender</value>
        public ISender Sender { get; }
        /// <summary>
        /// Gets the reoffer reference ticket id
        /// </summary>
        /// <value>The reoffer identifier</value>
        public string ReofferId { get; }
        /// <summary>
        /// Gets the alternative stake reference ticket id
        /// </summary>
        /// <value>The alt stake reference identifier</value>
        public string AltStakeRefId { get; }
        /// <summary>
        /// Gets a value indicating whether this is for testing
        /// </summary>
        /// <value><c>true</c> if [test source]; otherwise, <c>false</c></value>
        public bool TestSource { get; }
        /// <summary>
        /// Gets the type of the odds change Accept change in odds (optional, default none)
        /// <see cref="OddsChangeType.None" />: default behavior
        /// <see cref="OddsChangeType.Any" />: any odds change accepted
        /// <see cref="OddsChangeType.Higher" />: accept higher odds
        /// </summary>
        /// <value>The odds change</value>
        public OddsChangeType? OddsChange { get; }
        /// <summary>
        /// Gets the timestamp of ticket placement (UTC)
        /// </summary>
        /// <value>The timestamp</value>
        public DateTime Timestamp { get; }
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

        /// <summary>
        /// Gets the expected total number of generated combinations on this ticket (optional, default null). If present is used to validate against actual number of generated combinations.
        /// </summary>
        /// <value>The total combinations</value>
        public int? TotalCombinations { get; }

        /// <summary>
        /// Gets end time of last (non Sportradar) match on ticket.
        /// </summary>
        /// <value>End time of last (non Sportradar) match on ticket</value>
        public DateTime? LastMatchEndTime { get; }

        public string ToJson()
        {
            var dto = EntitiesMapper.Map(this);
            return dto.Ticket.ToJson();
        }

        [JsonConstructor]
        private Ticket(string ticketId, IEnumerable<IBet> bets, IEnumerable<ISelection> selections, ISender sender, string reofferId, string altStakeRefId, bool testSource, OddsChangeType? oddsChange, DateTime timestamp, string version, string correlationId, int? totalCombinations, DateTime? lastMatchEndTime)
        {
            TicketId = ticketId;
            Bets = bets;
            Selections = selections;
            Sender = sender;
            ReofferId = reofferId;
            AltStakeRefId = altStakeRefId;
            TestSource = testSource;
            OddsChange = oddsChange;
            Timestamp = timestamp;
            Version = version;
            CorrelationId = correlationId;
            TotalCombinations = totalCombinations;
            LastMatchEndTime = lastMatchEndTime;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ticket"/> class
        /// </summary>
        /// <param name="ticketId">The ticket identifier</param>
        /// <param name="sender">The sender</param>
        /// <param name="bets">The bets</param>
        /// <param name="reofferId">The reoffer identifier</param>
        /// <param name="altStakeRefId">The alt stake reference identifier</param>
        /// <param name="isTestSource">if set to <c>true</c> [is test source]</param>
        /// <param name="oddsChangeType">Type of the odds change</param>
        /// <exception cref="System.ArgumentException">Only ReofferId or AltStakeRefId can specified</exception>
        /// <param name="totalCombinations">Expected total number of generated combinations on this ticket (optional, default null). If present is used to validate against actual number of generated combinations</param>
        /// <param name="lastMatchEndTime">End time of last (non Sportradar) match on ticket.</param>
        public Ticket(string ticketId, ISender sender, IEnumerable<IBet> bets, string reofferId, string altStakeRefId, bool isTestSource, OddsChangeType? oddsChangeType, int? totalCombinations, DateTime? lastMatchEndTime)
        {
            Contract.Requires(TicketHelper.ValidateTicketId(ticketId));
            Contract.Requires(sender != null);
            Contract.Requires(bets != null);
            Contract.Requires(bets.Any() && bets.Count() <= 50);
            Contract.Requires(string.IsNullOrEmpty(reofferId) || TicketHelper.ValidateTicketId(reofferId));
            Contract.Requires(string.IsNullOrEmpty(altStakeRefId) || TicketHelper.ValidateTicketId(altStakeRefId));
            Contract.Requires(!(!string.IsNullOrEmpty(reofferId) && !string.IsNullOrEmpty(altStakeRefId)));

            TicketId = ticketId;
            Sender = sender;
            Bets = bets as IReadOnlyList<IBet>;
            ReofferId = reofferId;
            AltStakeRefId = altStakeRefId;
            TestSource = isTestSource;
            OddsChange = oddsChangeType;
            Timestamp = DateTime.UtcNow;
            Version = TicketHelper.MtsTicketVersion;
            CorrelationId = TicketHelper.GenerateTicketCorrelationId();
            LastMatchEndTime = lastMatchEndTime;

            if (!string.IsNullOrEmpty(reofferId) && !string.IsNullOrEmpty(altStakeRefId))
            {
                throw new ArgumentException("Only ReofferId or AltStakeRefId can be specified, not both.");
            }

            if (Bets != null)
            {
                var selections = new List<ISelection>();
                foreach (var bet in Bets)
                {
                    selections.AddRange(bet.Selections);
                }
                Selections = selections.Distinct();
            }
            TotalCombinations = totalCombinations;
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
            Contract.Invariant(Sender != null);
            Contract.Invariant(Bets != null);
            Contract.Invariant(Bets.Any() && Bets.Count() <= 50);
            Contract.Invariant(Selections != null);
            Contract.Invariant(Selections.Any() && Selections.Count() < 64);
            Contract.Invariant(string.IsNullOrEmpty(ReofferId) || TicketHelper.ValidateTicketId(ReofferId));
            Contract.Invariant(string.IsNullOrEmpty(AltStakeRefId) || TicketHelper.ValidateTicketId(AltStakeRefId));
            Contract.Invariant(!(!string.IsNullOrEmpty(ReofferId) && !string.IsNullOrEmpty(AltStakeRefId)));
            Contract.Invariant(TotalCombinations == null || TotalCombinations > 0);
            Contract.Invariant(LastMatchEndTime == null || LastMatchEndTime.Value > new DateTime(2000, 1, 1));
        }
    }
}