/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Enums;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Contracts
{
    [ContractClassFor(typeof(ITicketAck))]
    internal abstract class TicketAckContract : ITicketAck
    {
        public abstract DateTime Timestamp { get; }

        public abstract string TicketId { get; }

        public int BookmakerId
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() > 0);
                return Contract.Result<int>();
            }
        }

        public abstract string Version { get; }

        public abstract string CorrelationId { get; }

        public string ToJson()
        {
            Contract.Ensures(Contract.Result<string>().Length > 0);
            return Contract.Result<string>();
        }

        [Pure]
        public int Code => Contract.Result<int>();

        [Pure]
        public TicketAckStatus TicketStatus => Contract.Result<TicketAckStatus>();

        public string Message => Contract.Result<string>();
    }
}