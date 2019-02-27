/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */

using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Contracts;

namespace Sportradar.MTS.SDK.Entities.Interfaces
{
    /// <summary>
    /// Defines a contract for a customer confidence factor values for sport and prematch/live (if set for customer)
    /// </summary>
    [ContractClass(typeof(SportCcfContract))]
    public interface ISportCcf
    {
        /// <summary>
        /// Gets sport ID
        /// </summary>
        string SportId { get; }

        /// <summary>
        /// Gets customer confidence factor for the sport for prematch selections (factor multiplied by 10000)
        /// </summary>
        long PrematchCcf { get; }

        /// <summary>
        /// Gets customer confidence factor for the sport for live selections (factor multiplied by 10000)
        /// </summary>
        long LiveCcf { get; }
    }
}