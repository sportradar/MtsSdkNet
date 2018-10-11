/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Internal.TicketImpl
{
    public class SelectionRef : ISelectionRef
    {
        public int SelectionIndex { get; }
        public bool Banker { get; }

        public SelectionRef(int selectionIndex, bool isBanker)
        {
            Contract.Requires(selectionIndex >= 0 && selectionIndex <= 62);

            SelectionIndex = selectionIndex;
            Banker = isBanker;
        }

        /// <summary>
        /// Defines invariant members of the class
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(SelectionIndex >= 0 && SelectionIndex <= 62);
        }
    }
}