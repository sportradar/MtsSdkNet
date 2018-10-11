/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Contracts
{
    [ContractClassFor(typeof(IBetDetail))]
    internal abstract class BetDetailContract : IBetDetail
    {
        public string BetId
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                Contract.Ensures(Contract.Result<string>().Length > 0);
                Contract.Ensures(Contract.Result<string>().Length <= 128);
                return Contract.Result<string>();
            }
        }

        public IResponseReason Reason
        {
            get
            {
                Contract.Ensures(Contract.Result<IResponseReason>() != null);
                return Contract.Result<IResponseReason>();
            }
        }

        public IEnumerable<ISelectionDetail> SelectionDetails => Contract.Result<IEnumerable<ISelectionDetail>>();

        public IAlternativeStake AlternativeStake => Contract.Result<IAlternativeStake>();

        public IBetReoffer Reoffer => Contract.Result<IBetReoffer>();
    }
}

