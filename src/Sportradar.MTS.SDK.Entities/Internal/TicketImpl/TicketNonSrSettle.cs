using Sportradar.MTS.SDK.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sportradar.MTS.SDK.Entities.Internal.TicketImpl
{
    /// <summary>
    /// Implementation of <see cref="ITicketNonSrSettle"/>
    /// </summary>
    /// <seealso cref="ITicketNonSrSettle" />
    [Serializable]
    class TicketNonSrSettle : ITicketNonSrSettle
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
        /// Gets the non-sportradar settle stake
        /// </summary>
        /// <value>The non-sportradar settle stake</value>
        public long NonSrSettleStake { get; }
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
        /// Initializes a new instance of the <see cref="ITicketNonSrSettle"/> class
        /// </summary>
        /// <param name="ticketId">The ticket identifier</param>
        /// <param name="bookmakerId">The bookmaker identifier</param>
        /// <param name="stake">The non-sportradar settle stake</param>
        public TicketNonSrSettle(string ticketId, int bookmakerId, long stake)
        {
            Contract.Requires(TicketHelper.ValidateTicketId(ticketId));
            Contract.Requires(bookmakerId > 0);
            Contract.Requires(stake >= 0);

            TicketId = ticketId;
            BookmakerId = bookmakerId;
            NonSrSettleStake = stake;
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
            Contract.Invariant(NonSrSettleStake >= 0);
        }


    }
}
