/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;

// ReSharper disable UnusedMember.Local

namespace Sportradar.MTS.SDK.Entities.Internal
{
    /// <summary>
    /// Represents SDK configuration
    /// </summary>
    public class SdkConfigurationInternal : SdkConfiguration, ISdkConfigurationInternal
    {
        /// <summary>
        /// Default value for the InactivitySeconds property
        /// </summary>
        private const int AmpqDefaultInactivitySeconds = 180;

        /// <summary>
        /// Specifies minimum allowed value of the InactivitySeconds value
        /// </summary>
        private const int AmpqMinInactivitySeconds = 20;

        /// <summary>
        /// The ampq acknowledgment batch limit
        /// </summary>
        private const int AmpqAcknowledgmentBatchLimit = 10;

        /// <summary>
        /// The ampq acknowledgment timeout in seconds
        /// </summary>
        private const int AmpqAcknowledgmentTimeoutInSeconds = 60;

        /// <summary>
        /// Gets the URL of the feed's REST interface
        /// </summary>
        public string ApiHost { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdkConfiguration"/> class 
        /// </summary>
        public SdkConfigurationInternal(ISdkConfiguration config)
            : base(config.Username, config.Password, config.Host, config.VirtualHost, config.UseSsl, config.NodeId, config.BookmakerId, config.LimitId, config.Currency, config.Channel, config.AccessToken, config.ProvideAdditionalMarketSpecifiers, config.Port)
        {
            Contract.Requires(config != null);

            ApiHost = "https://api.betradar.com";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdkConfiguration"/> class
        /// </summary>
        /// <param name="section">A <see cref="SdkConfigurationSection"/> instance containing config values</param>
        internal SdkConfigurationInternal(ISdkConfigurationSection section)
            : base(section)
        {
            Contract.Requires(section != null);

            ApiHost = "https://api.betradar.com";
        }
    }
}