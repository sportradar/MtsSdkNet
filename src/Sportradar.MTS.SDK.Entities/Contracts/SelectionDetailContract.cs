/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Contracts
{
    [ContractClassFor(typeof(ISelectionDetail))]
    internal abstract class SelectionDetailContract : ISelectionDetail
    {
        public int SelectionIndex {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);
                Contract.Ensures(Contract.Result<int>() <= 62);
                return Contract.Result<int>();
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

        public IRejectionInfo RejectionInfo => Contract.Result<IRejectionInfo>();
    }
}