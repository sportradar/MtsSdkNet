/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Contracts;

namespace Sportradar.MTS.SDK.Entities.Interfaces
{
    /// <summary>
    /// Defines a contract for ticket selection
    /// </summary>
    [ContractClass(typeof(SelectionContract))]
    public interface ISelection
    {
        /// <summary>
        /// Gets the Betradar event (match or outright) id
        /// </summary>
        string EventId { get; }

        /// <summary>
        /// Gets the selection id
        /// </summary>
        /// <value>Should be composed according to specification</value>
        string Id { get; }

        /// <summary>
        /// Gets the odds multiplied by 10000 and rounded to int value
        /// </summary>
        int Odds { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is banker
        /// </summary>
        /// <value><c>true</c> if this instance is banker; otherwise, <c>false</c></value>
        bool IsBanker { get; }
    }
}