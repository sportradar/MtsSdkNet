/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Builders
{
    /// <summary>
    /// Defines a contract for classes building a <see cref="ITicketCashout" />
    /// </summary>
    public interface ITicketCashoutBuilder : ISdkTicketBuilder
    {
        /// <summary>
        /// Sets the ticket id to cashout
        /// </summary>
        /// <param name="ticketId">The ticket id</param>
        /// <returns>Returns a <see cref="ITicketCashoutBuilder"/></returns>
        /// <value>Unique ticket id (in the client's system)</value>
        ITicketCashoutBuilder SetTicketId(string ticketId);

        /// <summary>
        /// Sets the bookmaker id (client's id provided by Sportradar)
        /// </summary>
        /// <param name="bookmakerId">The bookmaker id</param>
        /// <returns>Returns a <see cref="ITicketCashoutBuilder"/></returns>
        ITicketCashoutBuilder SetBookmakerId(int bookmakerId);

        /// <summary>
        /// Sets the cashout stake
        /// </summary>
        /// <param name="stake">The cashout stake</param>
        /// <returns>Returns a <see cref="ITicketCashoutBuilder"/></returns>
        ITicketCashoutBuilder SetCashoutStake(long stake);

        /// <summary>
        /// Build a <see cref="ITicketCashout" />
        /// </summary>
        /// <param name="ticketId">The ticket id</param>
        /// <param name="bookmakerId">The bookmaker id</param>
        /// <param name="stake">The cashout stake</param>
        /// <returns>ITicketCashout</returns>
        ITicketCashout BuildTicket(string ticketId, int bookmakerId, long stake);

        /// <summary>
        /// Builds the <see cref="ITicketCashout" />
        /// </summary>
        /// <returns>Returns a <see cref="ITicketCashout"/></returns>
        ITicketCashout BuildTicket();
    }
}
