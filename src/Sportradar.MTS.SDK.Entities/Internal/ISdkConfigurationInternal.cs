/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Contracts;

namespace Sportradar.MTS.SDK.Entities.Internal
{
    /// <summary>
    /// Defines a contract implemented by classes representing odds feed configuration / settings
    /// </summary>
    [ContractClass(typeof(SdkConfigurationInternalContract))]
    public interface ISdkConfigurationInternal : ISdkConfiguration
    {
        /// <summary>
        /// Gets the URL of the feed's REST interface
        /// </summary>
        string ApiHost { get; }
    }
}