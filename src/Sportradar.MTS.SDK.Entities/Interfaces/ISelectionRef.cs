/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Contracts;

namespace Sportradar.MTS.SDK.Entities.Interfaces
{
    /// <summary>
    /// Defines a contract for selection references
    /// </summary>
    [ContractClass(typeof(SelectionRefContract))]
    public interface ISelectionRef
    {
        /// <summary>
        /// Gets the selection index from 'ticket.selections' array (zero based)
        /// </summary>
        int SelectionIndex { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ISelectionRef"/> is banker.
        /// </summary>
        bool Banker { get; }
    }
}