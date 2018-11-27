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
    [ContractClassFor(typeof(ITicketCancel))]
    internal abstract class TicketCancelContract : ITicketCancel
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

        [Pure]
        public TicketCancellationReason Code => Contract.Result<TicketCancellationReason>();

        public long? CancelPercent
        {
            get
            {
                Contract.Ensures(Contract.Result<long?>() == null || Contract.Result<long?>() > 0);
                return Contract.Result<long?>();
            }
        }

        public IEnumerable<IBetCancel> BetCancels
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<IBetCancel>>() == null || Contract.Result<IEnumerable<IBetCancel>>().Any());
                return Contract.Result<IEnumerable<IBetCancel>>();
            }
        }

        public string ToJson()
        {
            Contract.Ensures(Contract.Result<string>().Length > 0);
            return Contract.Result<string>();
        }
    }
}