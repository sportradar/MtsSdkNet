/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Sportradar.MTS.SDK.Entities.Enums;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Contracts
{
    [ContractClassFor(typeof(ITicketResponse))]
    internal abstract class TicketResponseContract : ITicketResponse
    {
        public abstract string TicketId { get; }

        public TicketAcceptance Status => Contract.Result<TicketAcceptance>();

        public IResponseReason Reason
        {
            get
            {
                Contract.Ensures(Contract.Result<IResponseReason>() != null);
                return Contract.Result<IResponseReason>();
            }
        }

        public IEnumerable<IBetDetail> BetDetails
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<IBetDetail>>().Any());
                return Contract.Result<IEnumerable<IBetDetail>>();
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

        public long ExchangeRate
        {
            get
            {
                Contract.Ensures(Contract.Result<long>() > 0);
                Contract.Ensures(Contract.Result<long>() < 1000000000000000000);
                return Contract.Result<long>();
            }
        }

        public IDictionary<string, string> AdditionalInfo => Contract.Result<IDictionary<string, string>>();

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