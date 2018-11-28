/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Enums;
using Sportradar.MTS.SDK.Entities.Interfaces;
using Sportradar.MTS.SDK.Entities.Internal;

namespace Sportradar.MTS.SDK.Entities.Contracts
{
    [ContractClassFor(typeof(ISender))]
    internal abstract class SenderContract : ISender
    {
        public int BookmakerId
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() > 0);
                return Contract.Result<int>();
            }
        }

        public string Currency
        {
            get
            {
                Contract.Ensures(Contract.Result<string>().Length == 3 || Contract.Result<string>().Length == 4);
                return Contract.Result<string>();
            }
        }

        public int LimitId
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() > 0);
                return Contract.Result<int>();
            }
        }

        public string TerminalId
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() == null || TicketHelper.ValidateUserId(Contract.Result<string>()));
                return Contract.Result<string>();
            }
        }

        public SenderChannel Channel => Contract.Result<SenderChannel>();

        public string ShopId
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() == null || TicketHelper.ValidateUserId(Contract.Result<string>()));
                return Contract.Result<string>();
            }
        }

        public IEndCustomer EndCustomer => Contract.Result<IEndCustomer>();
    }
}