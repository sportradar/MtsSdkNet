/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Builders;
using Sportradar.MTS.SDK.Entities.Enums;
using Sportradar.MTS.SDK.Entities.Interfaces;
using Sportradar.MTS.SDK.Entities.Internal.TicketImpl;

namespace Sportradar.MTS.SDK.Entities.Internal.Builders
{
    /// <summary>
    /// Implementation of the <see cref="ITicketCancelBuilder"/>
    /// </summary>
    /// <seealso cref="ITicketCancelBuilder" />
    public class TicketCancelBuilder : ITicketCancelBuilder
    {
        /// <summary>
        /// The ticket identifier
        /// </summary>
        private string _ticketId;

        /// <summary>
        /// The bookmaker identifier
        /// </summary>
        private int _bookmakerId;

        /// <summary>
        /// The code
        /// </summary>
        private TicketCancellationReason _code;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketCancelBuilder"/> class
        /// </summary>
        /// <param name="config">A <see cref="ISdkConfiguration"/> providing configuration for the associated sdk instance</param>
        internal TicketCancelBuilder(ISdkConfiguration config)
        {
            Contract.Requires(config != null);
            _bookmakerId = config.BookmakerId;
        }

        #region Obsolete_members
        /// <summary>
        /// Initializes a new instance of the <see cref="TicketCancelBuilder"/> class
        /// </summary>
        /// <param name="bookmakerId">The bookmaker identifier</param>
        internal TicketCancelBuilder(int bookmakerId = 0)
        {
            _bookmakerId = bookmakerId;
        }
       
        /// <summary>
        /// The <see cref="SdkConfigurationSection"/> loaded from app.config
        /// </summary>
        private static ISdkConfigurationSection _section;

        /// <summary>
        /// Value indicating whether an attempt to load the <see cref="_section"/> was already made
        /// </summary>
        private static bool _sectionLoaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketCancelBuilder"/> class
        /// </summary>
        /// <param name="bookmakerId">The bookmaker identifier</param>
        /// <returns>Returns an <see cref="ITicketCancelBuilder"/></returns>
        [Obsolete("Method Create(...) is obsolete. Please use the appropriate method on IBuilderFactory interface which can be obtained through MtsSdk instance")]
        public static ITicketCancelBuilder Create(int bookmakerId = 0)
        {
            if (!_sectionLoaded)
            {
                SdkConfigurationSection.TryGetSection(out _section);
                _sectionLoaded = true;
            }

            if (_section != null && bookmakerId == 0)
            {
                try
                {
                    var config = SdkConfigurationSection.GetSection();
                    bookmakerId = config.BookmakerId;
                }
                catch (Exception)
                {
                    // if exists, try to load, otherwise user must explicitly set it
                }
            }
            return new TicketCancelBuilder(bookmakerId);
        }
        #endregion

        /// <summary>
        /// Sets the ticket id to cancel
        /// </summary>
        /// <param name="ticketId">The ticket id</param>
        /// <returns>Returns a <see cref="ITicketCancelBuilder" /></returns>
        /// <value>Unique ticket id (in the client's system)</value>
        public ITicketCancelBuilder SetTicketId(string ticketId)
        {
            _ticketId = ticketId;
            return this;
        }

        /// <summary>
        /// Get the bookmaker id (client's id provided by Sportradar)
        /// </summary>
        /// <param name="bookmakerId">The bookmaker id</param>
        /// <returns>Returns a <see cref="ITicketCancelBuilder" /></returns>
        public ITicketCancelBuilder SetBookmakerId(int bookmakerId)
        {
            _bookmakerId = bookmakerId;
            return this;
        }

        /// <summary>
        /// Sets the cancellation code
        /// </summary>
        /// <param name="code">The <see cref="TicketCancellationReason" /></param>
        /// <returns>Returns a <see cref="ITicketCancelBuilder" /></returns>
        public ITicketCancelBuilder SetCode(TicketCancellationReason code)
        {
            _code = code;
            return this;
        }

        /// <summary>
        /// Builds the <see cref="ITicketCancel" />
        /// </summary>
        /// <returns>Returns a <see cref="ITicketCancel" /></returns>
        public ITicketCancel BuildTicket()
        {
            return new TicketCancel(_ticketId, _bookmakerId, _code);
        }

        /// <summary>
        /// Build a <see cref="ITicketCancel" />
        /// </summary>
        /// <param name="ticketId">The ticket id</param>
        /// <param name="bookmakerId">The bookmaker id</param>
        /// <param name="code">The <see cref="TicketCancellationReason" /></param>
        /// <returns>Return an <see cref="ITicketCancel"/></returns>
        public ITicketCancel BuildTicket(string ticketId, int bookmakerId, TicketCancellationReason code)
        {
            return new TicketCancel(ticketId, bookmakerId, code);
        }
    }
}