/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Internal.TicketImpl
{
    public class SelectionDetail : ISelectionDetail
    {
        public int SelectionIndex { get; }
        public IResponseReason Reason { get; }
        public IRejectionInfo RejectionInfo { get; }

        public SelectionDetail(int selectionIndex, IResponseReason reason, IRejectionInfo rejectionInfo)
        {
            Contract.Requires(selectionIndex >= 0 && selectionIndex <= 62);
            Contract.Requires(reason != null);

            SelectionIndex = selectionIndex;
            Reason = reason;
            RejectionInfo = rejectionInfo;
        }

        /// <summary>
        /// Defines invariant members of the class
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(SelectionIndex >= 0 && SelectionIndex <= 62);
            Contract.Invariant(Reason != null);
        }
    }
}