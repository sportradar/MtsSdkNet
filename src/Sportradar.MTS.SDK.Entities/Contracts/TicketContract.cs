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

        /// <summary>
        /// Gets the expected total number of generated combinations on this ticket (optional, default null). If present is used to validate against actual number of generated combinations.
        /// </summary>
        /// <value>The total combinations</value>
        public int? TotalCombinations {
            get
            {
                Contract.Ensures(Contract.Result<int?>() == null ||
                                 (Contract.Result<int?>() > 0));
                return Contract.Result<int?>();
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
                Contract.Ensures(Contract.Result<string>() == null
                                || (Contract.Result<string>().Length >= 1
                                && Contract.Result<string>().Length <= 128));
                return Contract.Result<string>();
            }
        }

        public DateTime? LastMatchEndTime
        {
            get
            {
                Contract.Ensures(Contract.Result<DateTime?>() == null || Contract.Result<DateTime?>().Value > new DateTime(2000, 1, 1));
                return Contract.Result<DateTime?>();
            }
        }

        public string ToJson()
        {
            Contract.Ensures(Contract.Result<string>().Length > 0);
            return Contract.Result<string>();
        }
    }
}