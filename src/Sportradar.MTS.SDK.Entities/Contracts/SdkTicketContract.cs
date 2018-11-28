/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Interfaces;
using Sportradar.MTS.SDK.Entities.Internal;

namespace Sportradar.MTS.SDK.Entities.Contracts
{
    [ContractClassFor(typeof(ISdkTicket))]
    internal abstract class SdkTicketContract : ISdkTicket
    {
        public string TicketId
        {
            get
            {
                Contract.Ensures(Contract.Result<string>().Length > 0);
                Contract.Ensures(Contract.Result<string>().Length <= 128);
                Contract.Ensures(TicketHelper.ValidateTicketId(Contract.Result<string>()));
                return Contract.Result<string>();
            }
        }

        public DateTime Timestamp
        {
            get
            {
                Contract.Ensures(Contract.Result<DateTime>() > DateTime.MinValue);
                return Contract.Result<DateTime>();
            }
        }
        
        public string Version
        {
            get
            {
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));
                Contract.Ensures(Contract.Result<string>().Length == 3);
                return Contract.Result<string>();
            }
        }

        public string CorrelationId
        {
            get
            {
                Contract.Ensures(Contract.Result<string>().Length > 0);
                Contract.Ensures(Contract.Result<string>().Length <= 128);
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