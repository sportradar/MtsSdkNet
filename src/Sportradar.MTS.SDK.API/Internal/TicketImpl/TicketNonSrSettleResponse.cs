﻿/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.API.Internal.Senders;
using Sportradar.MTS.SDK.Entities.Enums;
using Sportradar.MTS.SDK.Entities.Interfaces;
using Sportradar.MTS.SDK.Entities.Internal;
using System.Collections.Generic;

namespace Sportradar.MTS.SDK.API.Internal.TicketImpl
{
    /// <summary>
    /// Implementation of <see cref="ITicketNonSrSettleResponse"/>
    /// </summary>
    /// <seealso cref="ITicketNonSrSettleResponse" />
    [Serializable]
    public class TicketNonSrSettleResponse : ITicketNonSrSettleResponse
    {
        /// <summary>
        /// The ticket Non-Sportaradar settle sender
        /// </summary>
        // ReSharper disable once NotAccessedField.Local
        private readonly ITicketSender _ticketNonSrSettle;
        /// <summary>
        /// Gets the ticket id
        /// </summary>
        /// <value>Unique ticket id (in the client's system)</value>
        public string TicketId { get; }
        /// <summary>
        /// Gets the response reason
        /// </summary>
        /// <value>The reason</value>
        public IResponseReason Reason { get; }
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
        /// Gets the response signature/hash (previous BetAcceptanceId)
        /// </summary>
        /// <value>The signature</value>
        public string Signature { get; }

        /// <summary>
        /// Gets the additional information about the response
        /// </summary>
        /// <value>The additional information</value>
        /// <remarks>Contains timestamps describing mts processing (receivedUtcTimestamp, validatedUtcTimestamp, respondedUtcTimestamp)</remarks>
        public IDictionary<string, string> AdditionalInfo { get; }

        /// <summary>
        /// Gets the timestamp of ticket placement (UTC)
        /// </summary>
        /// <value>The timestamp</value>
        public DateTime Timestamp { get; }
        /// <summary>
        /// Gets the status of the ticket cashout submission
        /// </summary>
        /// <value>The status</value>
        public NonSrSettleAcceptance Status { get; }

        private readonly string _originalJson;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketNonSrSettleResponse"/> class
        /// </summary>
        /// <param name="ticketNonSrSetter">The ticket non-sr settle sender</param>
        /// <param name="ticketId">The ticket identifier</param>
        /// <param name="status">The status</param>
        /// <param name="reason">The reason</param>
        /// <param name="correlationId">The correlation id</param>
        /// <param name="signature">The signature</param>
        /// <param name="version">The version</param>
        /// <param name="additionalInfo">The additional information</param>
        /// <param name="orgJson">The original json string received from the mts</param>
        public TicketNonSrSettleResponse(ITicketSender ticketNonSrSetter,
                                    string ticketId,
                                    NonSrSettleAcceptance status,
                                    IResponseReason reason,
                                    string correlationId,
                                    string signature,
                                    string version = TicketHelper.MtsTicketVersion,
                                    IDictionary<string, string> additionalInfo = null,
                                    string orgJson = null)
        {
            TicketId = ticketId;
            Status = status;
            Reason = reason;
            Signature = signature;
            Version = version;
            Timestamp = DateTime.UtcNow;
            CorrelationId = correlationId;
            AdditionalInfo = additionalInfo;
            _originalJson = orgJson;

            _ticketNonSrSettle = ticketNonSrSetter;
        }

        /// <summary>
        /// Defines invariant members of the class
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(!string.IsNullOrEmpty(Version));
            Contract.Invariant(Timestamp > DateTime.MinValue);
            Contract.Invariant(!string.IsNullOrEmpty(Signature));
            Contract.Invariant(Reason != null);
        }

        /// <summary>
        /// Send acknowledgment back to MTS
        /// </summary>
        /// <param name="markAccepted">if set to <c>true</c> [mark accepted]</param>
        /// <param name="bookmakerId">The sender identifier (bookmakerId)</param>
        /// <param name="code">The code</param>
        /// <param name="message">The message</param>
        /// <exception cref="NullReferenceException">Missing TicketCashoutSender. Can not be null</exception>
        public void Acknowledge(bool markAccepted, int bookmakerId, int code, string message)
        {
            // acking is not supported
        }

        /// <summary>
        /// Send acknowledgment back to MTS
        /// </summary>
        /// <param name="markAccepted">if set to <c>true</c> [mark accepted]</param>
        /// <exception cref="System.Exception">missing ticket in cache</exception>
        public void Acknowledge(bool markAccepted = true)
        {
            // acking is not supported
        }

        public string ToJson()
        {
            return _originalJson;
        }
    }
}
