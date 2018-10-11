/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Contracts;
using Sportradar.MTS.SDK.Entities.Enums;

namespace Sportradar.MTS.SDK.Entities.Interfaces
{
    /// <summary>
    /// Contract defining Ticket that can be send to the MTS
    /// </summary>
    [ContractClass(typeof(TicketContract))]
    public interface ITicket : ISdkTicket
    {
        /// <summary>
        /// Gets the collection of all bets
        /// </summary>
        [Required]
        IEnumerable<IBet> Bets { get; }

        /// <summary>
        /// Gets the identification and settings of the ticket sender
        /// </summary>
        [Required]
        ISender Sender { get; }

        /// <summary>
        /// Gets the reoffer reference ticket id
        /// </summary>
        string ReofferId { get; }

        /// <summary>
        /// Gets the alternative stake reference ticket id
        /// </summary>
        string AltStakeRefId { get; }

        /// <summary>
        /// Gets a value indicating whether this is for testing
        /// </summary>
        bool TestSource { get; }

        /// <summary>
        /// Gets the type of the odds change Accept change in odds (optional, default none) 
        /// <see cref="OddsChangeType.None"/>: default behavior
        /// <see cref="OddsChangeType.Any"/>: any odds change accepted
        /// <see cref="OddsChangeType.Higher"/>: accept higher odds
        /// </summary>
        OddsChangeType? OddsChange { get; }

        /// <summary>
        /// Gets the collection of all selections
        /// </summary>
        [Required]
        IEnumerable<ISelection> Selections { get; }
    }
}