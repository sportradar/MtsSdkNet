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
    [ContractClassFor(typeof(ITicket))]
    internal abstract class TicketContract : ITicket
    {
        public abstract string TicketId { get; }

        public IEnumerable<IBet> Bets
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<IBet>>().Any());
                Contract.Ensures(Contract.Result<IEnumerable<IBet>>().Count() <= 50);
                return Contract.Result<IEnumerable<IBet>>();
            }
        }

        public IEnumerable<ISelection> Selections
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<ISelection>>().Any());
                Contract.Ensures(Contract.Result<IEnumerable<ISelection>>().Count() <= 63);
                return Contract.Result<IEnumerable<ISelection>>();
            }
        }

        public ISender Sender
        {
            get
            {
                Contract.Ensures(Contract.Result<ISender>() != null);
                return Contract.Result<ISender>();
            }
        }

        public string ReofferId
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() == null ||
                                (Contract.Result<string>().Length >= 1 
                                && Contract.Result<string>().Length <= 128));
                return Contract.Result<string>();
            }
        }

        public bool TestSource => Contract.Result<bool>();

        public OddsChangeType? OddsChange => Contract.Result<OddsChangeType?>();

        public abstract DateTime Timestamp { get; }

        public abstract string Version { get; }

        public abstract string CorrelationId { get; }

        public string AltStakeRefId
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() == null ||
                                (Contract.Result<string>().Length >= 1
                                && Contract.Result<string>().Length <= 128));
                return Contract.Result<string>();
            }
        }

        public string ToJson()
        {
            Contract.Ensures(Contract.Result<string>().Length > 0);
            return Contract.Result<string>();
        }
    }
}