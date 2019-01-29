/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Enums;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Contracts
{
    [ContractClassFor(typeof(ITicketCashoutResponse))]
    internal abstract class TicketCashoutResponseContract : ITicketCashoutResponse
    {
        public abstract string TicketId { get; }

        public CashoutAcceptance Status => Contract.Result<CashoutAcceptance>();

        public IResponseReason Reason
        {
            get
            {
                Contract.Ensures(Contract.Result<IResponseReason>() != null);
                return Contract.Result<IResponseReason>();
            }
        }

        public abstract string Version { get; }

        public abstract string CorrelationId { get; }

        public string Signature
        {
            get
            {
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));
                return Contract.Result<string>();
            }
        }

        public abstract void Acknowledge(bool markAccepted, int bookmakerId, int code, string message);

        public abstract void Acknowledge(bool markAccepted = true);

        public abstract DateTime Timestamp { get; }

        public string ToJson()
        {
            Contract.Ensures(Contract.Result<string>().Length > 0);
            return Contract.Result<string>();
        }
    }
}