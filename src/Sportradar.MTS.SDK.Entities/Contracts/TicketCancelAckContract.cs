/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Enums;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Contracts
{
    [ContractClassFor(typeof(ITicketCancelAck))]
    internal abstract class TicketCancelAckContract : ITicketCancelAck
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

        public int Code => Contract.Result<int>();

        public TicketCancelAckStatus TicketCancelStatus => Contract.Result<TicketCancelAckStatus>();

        public string Message => Contract.Result<string>();

        public string ToJson()
        {
            Contract.Ensures(Contract.Result<string>().Length > 0);
            return Contract.Result<string>();
        }
    }
}