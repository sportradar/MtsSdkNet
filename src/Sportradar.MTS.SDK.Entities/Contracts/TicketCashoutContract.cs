/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Contracts
{
    [ContractClassFor(typeof(ITicketCashout))]
    internal abstract class TicketCashoutContract : ITicketCashout
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

        public long CashoutStake
        {
            get
            {
                Contract.Ensures(Contract.Result<long>() > 0);
                return Contract.Result<long>();
            }
        }

        public abstract string Version { get; }

        public abstract string CorrelationId { get; }

        public string ToJson()
        {
            Contract.Ensures(Contract.Result<string>().Length > 0);
            return Contract.Result<string>();
        }
    }
}