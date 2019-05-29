/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities;
using Sportradar.MTS.SDK.Entities.Enums;

namespace Sportradar.MTS.SDK.API.Contracts
{
    [ContractClassFor(typeof(ISdkConfigurationBuilder))]
    internal abstract class SdkConfigurationBuilderContract : ISdkConfigurationBuilder
    {
        public ISdkConfigurationBuilder SetUsername(string username)
        {
            Contract.Requires(!string.IsNullOrEmpty(username));
            Contract.Ensures(Contract.Result<ISdkConfigurationBuilder>() != null);
            return Contract.Result<ISdkConfigurationBuilder>();
        }

        public ISdkConfigurationBuilder SetPassword(string password)
        {
            Contract.Requires(!string.IsNullOrEmpty(password));
            Contract.Ensures(Contract.Result<ISdkConfigurationBuilder>() != null);
            return Contract.Result<ISdkConfigurationBuilder>();
        }

        public ISdkConfigurationBuilder SetHost(string host)
        {
            Contract.Requires(!string.IsNullOrEmpty(host));
            Contract.Requires(!host.Contains(":"));
            Contract.Ensures(Contract.Result<ISdkConfigurationBuilder>() != null);
            return Contract.Result<ISdkConfigurationBuilder>();
        }

        public ISdkConfigurationBuilder SetPort(int port)
        {
            Contract.Requires(port > 0);
            Contract.Ensures(Contract.Result<ISdkConfigurationBuilder>() != null);
            return Contract.Result<ISdkConfigurationBuilder>();
        }

        public ISdkConfigurationBuilder SetVirtualHost(string vhost)
        {
            Contract.Requires(!string.IsNullOrEmpty(vhost));
            Contract.Ensures(Contract.Result<ISdkConfigurationBuilder>() != null);
            return Contract.Result<ISdkConfigurationBuilder>();
        }

        public ISdkConfigurationBuilder SetNode(int nodeId)
        {
            Contract.Requires(nodeId > 0);
            Contract.Ensures(Contract.Result<ISdkConfigurationBuilder>() != null);
            return Contract.Result<ISdkConfigurationBuilder>();
        }

        public ISdkConfigurationBuilder SetUseSsl(bool useSsl)
        {
            Contract.Ensures(Contract.Result<ISdkConfigurationBuilder>() != null);
            return Contract.Result<ISdkConfigurationBuilder>();
        }

        public ISdkConfigurationBuilder SetBookmakerId(int bookmakerId)
        {
            Contract.Requires(bookmakerId > 0);
            Contract.Ensures(Contract.Result<ISdkConfigurationBuilder>() != null);
            return Contract.Result<ISdkConfigurationBuilder>();
        }

        public ISdkConfigurationBuilder SetLimitId(int limitId)
        {
            Contract.Requires(limitId > 0);
            Contract.Ensures(Contract.Result<ISdkConfigurationBuilder>() != null);
            return Contract.Result<ISdkConfigurationBuilder>();
        }

        public ISdkConfigurationBuilder SetCurrency(string currency)
        {
            Contract.Requires(!string.IsNullOrEmpty(currency));
            Contract.Requires(currency.Length == 3 || currency.Length == 4);
            Contract.Ensures(Contract.Result<ISdkConfigurationBuilder>() != null);
            return Contract.Result<ISdkConfigurationBuilder>();
        }

        public ISdkConfigurationBuilder SetSenderChannel(SenderChannel channel)
        {
            Contract.Ensures(Contract.Result<ISdkConfigurationBuilder>() != null);
            return Contract.Result<ISdkConfigurationBuilder>();
        }

        public ISdkConfigurationBuilder SetAccessToken(string accessToken)
        {
            Contract.Requires(!string.IsNullOrEmpty(accessToken));
            Contract.Ensures(Contract.Result<ISdkConfigurationBuilder>() != null);
            return Contract.Result<ISdkConfigurationBuilder>();
        }

        public ISdkConfigurationBuilder SetUfEnvironment(UfEnvironment ufEnvironment)
        {
            Contract.Ensures(Contract.Result<ISdkConfigurationBuilder>() != null);
            return Contract.Result<ISdkConfigurationBuilder>();
        }

        public ISdkConfigurationBuilder SetProvideAdditionalMarketSpecifiers(bool provideAdditionalMarketSpecifiers)
        {
            Contract.Ensures(Contract.Result<ISdkConfigurationBuilder>() != null);
            return Contract.Result<ISdkConfigurationBuilder>();
        }

        /// <summary>
        /// Sets the value indicating whether the rabbit consumer channel should be exclusive
        /// </summary>
        /// <param name="exclusiveConsumer">The value to be set</param>
        /// <returns>Returns a <see cref="ISdkConfigurationBuilder"/></returns>
        public ISdkConfigurationBuilder SetExclusiveConsumer(bool exclusiveConsumer)
        {
            Contract.Ensures(Contract.Result<ISdkConfigurationBuilder>() != null);
            return Contract.Result<ISdkConfigurationBuilder>();
        }

        public ISdkConfigurationBuilder SetKeycloakHost(string keycloakHost)
        {
            Contract.Requires(!string.IsNullOrEmpty(keycloakHost));
            Contract.Ensures(Contract.Result<ISdkConfigurationBuilder>() != null);
            return Contract.Result<ISdkConfigurationBuilder>();
        }

        public ISdkConfigurationBuilder SetKeycloakUsername(string keycloakUsername)
        {
            Contract.Requires(!string.IsNullOrEmpty(keycloakUsername));
            Contract.Ensures(Contract.Result<ISdkConfigurationBuilder>() != null);
            return Contract.Result<ISdkConfigurationBuilder>();
        }

        public ISdkConfigurationBuilder SetKeycloakPassword(string keycloakPassword)
        {
            Contract.Requires(!string.IsNullOrEmpty(keycloakPassword));
            Contract.Ensures(Contract.Result<ISdkConfigurationBuilder>() != null);
            return Contract.Result<ISdkConfigurationBuilder>();
        }

        public ISdkConfigurationBuilder SetKeycloakSecret(string keycloakSecret)
        {
            Contract.Requires(!string.IsNullOrEmpty(keycloakSecret));
            Contract.Ensures(Contract.Result<ISdkConfigurationBuilder>() != null);
            return Contract.Result<ISdkConfigurationBuilder>();
        }

        public ISdkConfigurationBuilder SetMtsClientApiHost(string mtsClientApiHost)
        {
            Contract.Requires(!string.IsNullOrEmpty(mtsClientApiHost));
            Contract.Ensures(Contract.Result<ISdkConfigurationBuilder>() != null);
            return Contract.Result<ISdkConfigurationBuilder>();
        }

        public ISdkConfigurationBuilder SetTicketResponseTimeout(int responseTimeout)
        {
            Contract.Ensures(Contract.Result<ISdkConfigurationBuilder>() != null);
            return Contract.Result<ISdkConfigurationBuilder>();
        }

        public ISdkConfigurationBuilder SetTicketCancellationResponseTimeout(int responseTimeout)
        {
            Contract.Ensures(Contract.Result<ISdkConfigurationBuilder>() != null);
            return Contract.Result<ISdkConfigurationBuilder>();
        }

        public ISdkConfigurationBuilder SetTicketCashoutResponseTimeout(int responseTimeout)
        {
            Contract.Ensures(Contract.Result<ISdkConfigurationBuilder>() != null);
            return Contract.Result<ISdkConfigurationBuilder>();
        }

        public ISdkConfigurationBuilder SetNonSrSettleResponseTimeout(int responseTimeout)
        {
            Contract.Ensures(Contract.Result<ISdkConfigurationBuilder>() != null);
            return Contract.Result<ISdkConfigurationBuilder>();
        }

        public ISdkConfiguration Build()
        {
            Contract.Ensures(Contract.Result<ISdkConfiguration>() != null);
            return Contract.Result<ISdkConfiguration>();
        }
    }
}
