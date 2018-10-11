/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using Sportradar.MTS.SDK.Entities.Enums;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Builders
{
    /// <summary>
    /// Defines a contract for classes building a <see cref="ITicketCancel" />
    /// </summary>
    public interface ITicketCancelBuilder : ISdkTicketBuilder
    {
        /// <summary>
        /// Sets the ticket id to cancel
        /// </summary>
        /// <param name="ticketId">The ticket id</param>
        /// <returns>Returns a <see cref="ITicketCancelBuilder"/></returns>
        /// <value>Unique ticket id (in the client's system)</value>
        ITicketCancelBuilder SetTicketId(string ticketId);

        /// <summary>
        /// Get the bookmaker id (client's id provided by Sportradar)
        /// </summary>
        /// <param name="bookmakerId">The bookmaker id</param>
        /// <returns>Returns a <see cref="ITicketCancelBuilder"/></returns>
        ITicketCancelBuilder SetBookmakerId(int bookmakerId);

        /// <summary>
        /// Sets the cancellation code
        /// </summary>
        /// <param name="code">The <see cref="TicketCancellationReason"/></param>
        /// <returns>Returns a <see cref="ITicketCancelBuilder"/></returns>
        ITicketCancelBuilder SetCode(TicketCancellationReason code);

        /// <summary>
        /// Build a <see cref="ITicketCancel" />
        /// </summary>
        /// <param name="ticketId">The ticket id</param>
        /// <param name="bookmakerId">The bookmaker id</param>
        /// <param name="code">The <see cref="TicketCancellationReason"/></param>
        /// <returns>ITicketCancel</returns>
        ITicketCancel BuildTicket(string ticketId, int bookmakerId, TicketCancellationReason code);

        /// <summary>
        /// Builds the <see cref="ITicketCancel" />
        /// </summary>
        /// <returns>Returns a <see cref="ITicketCancel"/></returns>
        ITicketCancel BuildTicket();
    }
}
