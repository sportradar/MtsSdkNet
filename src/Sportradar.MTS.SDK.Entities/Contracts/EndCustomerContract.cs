/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Interfaces;
using Sportradar.MTS.SDK.Entities.Internal;

namespace Sportradar.MTS.SDK.Entities.Contracts
{
    [ContractClassFor(typeof(IEndCustomer))]
    internal abstract class EndCustomerContract : IEndCustomer
    {
        public string Ip
        {
            get
            {
                return Contract.Result<string>();
            }
        }

        public string LanguageId {
            get
            {
                Contract.Ensures(Contract.Result<string>() == null ||
                                 Contract.Result<string>().Length == 2);
                return Contract.Result<string>();
            }
        }

        public string DeviceId
        {
            get
            {
                Contract.Ensures(TicketHelper.ValidateUserId(Contract.Result<string>()));
                return Contract.Result<string>();
            }
        }

        public string Id {
            get
            {
                Contract.Ensures(TicketHelper.ValidateUserId(Contract.Result<string>()));
                return Contract.Result<string>();
            }
        }

        public long Confidence {
            get
            {
                Contract.Ensures(Contract.Result<long>() >= 0);
                return Contract.Result<long>();
            }
        }
    }
}

