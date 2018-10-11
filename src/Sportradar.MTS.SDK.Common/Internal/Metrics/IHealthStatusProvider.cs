/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Metrics;
using Sportradar.MTS.SDK.Common.Contracts;

namespace Sportradar.MTS.SDK.Common.Internal.Metrics
{
    /// <summary>
    /// Defines a contract implemented by classes used to provide <see cref="HealthCheckResult"/> for the SDK
    /// </summary>
    [ContractClass(typeof(HealthStatusProviderContract))]
    public interface IHealthStatusProvider
    {
        /// <summary>
        /// Registers the health check which will be periodically triggered
        /// </summary>
        void RegisterHealthCheck();

        /// <summary>
        /// Starts the health check and returns <see cref="HealthCheckResult"/>
        /// </summary>
        HealthCheckResult StartHealthCheck();
    }
}
