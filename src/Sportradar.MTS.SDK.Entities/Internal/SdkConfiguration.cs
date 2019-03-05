/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */

using System;
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Common.Internal;
using Sportradar.MTS.SDK.Entities.Enums;

namespace Sportradar.MTS.SDK.Entities.Internal
{
    /// <summary>
    /// Represents SDK configuration
    /// </summary>
    /// <seealso cref="ISdkConfiguration" />
    public class SdkConfiguration : ISdkConfiguration
    {
        /// <summary>
        /// Gets an username used when establishing connection to the AMQP broker
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// Gets a password used when establishing connection to the AMQP broker
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// Gets a value specifying the host name of the AMQP broker
        /// </summary>
        public string Host { get; }

        /// <summary>
        /// Gets the port number used to connect to the AMQP broker
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Gets a value specifying the virtual host name of the AMQP broker
        /// </summary>
        public string VirtualHost { get; }

        /// <summary>
        /// Gets a value specifying whether the connection to AMQP broker should use SSL encryption
        /// </summary>
        public bool UseSsl { get; }

        /// <summary>
        /// Gets a node id
        /// </summary>
        public int NodeId { get; }

        /// <summary>
        /// Gets the BookmakerId associated with the current configuration or 0 if none is provided
        /// </summary>
        public int BookmakerId { get; }

        /// <summary>
        /// Gets the channel identifier associated with the current configuration or 0 if none is provided
        /// </summary>
        public int LimitId { get; }

        /// <summary>
        /// Gets the default currency associated with the current configuration or a null reference if none is provided.
        /// </summary>
        public string Currency { get; }

        /// <summary>
        /// Gets the <see cref="SenderChannel" /> specifying the associated channel or a null reference if none is specified.
        /// </summary>
        public SenderChannel? Channel { get; }

        /// <summary>
        /// Gets a value indication whether statistics collection is enabled
        /// </summary>
        public bool StatisticsEnabled { get; }

        /// <summary>
        /// Gets the timeout for automatically collecting statistics (in sec)
        /// </summary>
        public int StatisticsTimeout { get; }

        /// <summary>
        /// Gets the limit of records for automatically writing statistics (in number of records)
        /// </summary>
        public int StatisticsRecordLimit { get; }

        /// <summary>
        /// Gets the access token for the UF feed (only necessary if UF selections will be build)
        /// </summary>
        /// <value>The access token</value>
        public string AccessToken { get; }

        /// <summary>
        /// Gets a value indicating whether additional market specifiers should be added
        /// </summary>
        /// <value><c>true</c> if [provide additional market specifiers]; otherwise, <c>false</c></value>
        public bool ProvideAdditionalMarketSpecifiers { get; }

        /// <summary>
        /// Should the rabbit consumer channel be exclusive
        /// </summary>
        public bool ExclusiveConsumer { get; }

        /// <summary>
        /// Gets the Keycloak host for authorization
        /// </summary>
        public string KeycloakHost { get; }

        /// <summary>
        /// Gets the username used to connect authenticate to Keycloak
        /// </summary>
        public string KeycloakUsername { get; }

        /// <summary>
        /// Gets the password used to connect authenticate to Keycloak
        /// </summary>
        public string KeycloakPassword { get; }

        /// <summary>
        /// Gets the secret used to connect authenticate to Keycloak
        /// </summary>
        public string KeycloakSecret { get; }

        /// <summary>
        /// Gets the Client API host
        /// </summary>
        public string MtsClientApiHost { get; }

        /// <summary>
        /// Gets the ticket response timeout(ms)
        /// </summary>
        public int TicketResponseTimeout { get; }

        /// <summary>
        /// Gets the ticket cancellation response timeout(ms)
        /// </summary>
        public int TicketCancellationResponseTimeout { get; }

        /// <summary>
        /// Gets the ticket cashout response timeout(ms)
        /// </summary>
        public int TicketCashoutResponseTimeout { get; }

        /// <summary>
        /// Gets the ticket non-sportradar settle response timeout(ms)
        /// </summary>
        public int TicketNonSrSettleResponseTimeout { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="SdkConfiguration"/> class
        /// </summary>
        /// <param name="username">The username used when connecting to AMQP broker</param>
        /// <param name="password">The password used when connecting to AMQP broker</param>
        /// <param name="host">The host name of the AMQP broker</param>
        /// <param name="vhost">The virtual host defined on the AMQP broker</param>
        /// <param name="useSsl">Value indicating whether SSL should be used when connecting to AMQP</param>
        /// <param name="nodeId"> The value uniquely identifying the SDK instance associated with the current configuration</param>
        /// <param name="bookmakerId">The bookmaker id assigned to the customer by the MTS CI</param>
        /// <param name="limitId">The value specifying the limits of the placed tickets</param>
        /// <param name="currency">The currency of the placed tickets or a null reference</param>
        /// <param name="channel">The <see cref="SenderChannel"/> specifying the origin of the tickets or a null reference</param>
        /// <param name="accessToken">The access token for the UF feed (only necessary if UF selections will be build)</param>
        /// <param name="provideAdditionalMarketSpecifiers">The value indicating if the additional market specifiers should be provided</param>
        /// <param name="port">The port number used to connect to the AMQP broker</param>
        /// <param name="exclusiveConsumer">Should the consumer channel be exclusive</param>
        /// <param name="keycloakHost">The Keycloak host for authorization</param>
        /// <param name="keycloakUsername">The username used to connect authenticate to Keycloak</param>
        /// <param name="keycloakPassword">The password used to connect authenticate to Keycloak</param>
        /// <param name="keycloakSecret">The secret used to connect authenticate to Keycloak</param>
        /// <param name="mtsClientApiHost">The Client API host</param>
        /// <param name="ticketResponseTimeout">The ticket response timeout(ms)</param>
        /// <param name="ticketCancellationResponseTimeout">The ticket cancellation response timeout(ms)</param>
        /// <param name="ticketCashoutResponseTimeout">The ticket cashout response timeout(ms)</param>
        /// <param name="ticketNonSrSettleResponseTimeout">The ticket cashout response timeout(ms)</param>
        public SdkConfiguration(
            string username,
            string password,
            string host,
            string vhost = null,
            bool useSsl = true,
            int nodeId = 1,
            int bookmakerId = 0,
            int limitId = 0,
            string currency = null,
            SenderChannel? channel = null,
            string accessToken = null,
            bool provideAdditionalMarketSpecifiers = true,
            int port = 0,
            bool exclusiveConsumer = true,
            string keycloakHost = null,
            string keycloakUsername = null,
            string keycloakPassword = null,
            string keycloakSecret = null,
            string mtsClientApiHost = null,
            int ticketResponseTimeout = SdkInfo.TicketResponseTimeoutDefault,
            int ticketCancellationResponseTimeout = SdkInfo.TicketCancellationResponseTimeoutDefault,
            int ticketCashoutResponseTimeout = SdkInfo.TicketCashoutResponseTimeoutDefault,
            int ticketNonSrSettleResponseTimeout = SdkInfo.TicketCashoutResponseTimeoutDefault)
        {
            Contract.Requires(!string.IsNullOrEmpty(username));
            Contract.Requires(!string.IsNullOrEmpty(password));
            Contract.Requires(!string.IsNullOrEmpty(host));

            if (ticketResponseTimeout < SdkInfo.TicketResponseTimeoutMin)
            {
                throw new ArgumentException($"TicketResponseTimeout must be more than {SdkInfo.TicketCashoutResponseTimeoutMin}ms");
            }
            if (ticketResponseTimeout > SdkInfo.TicketResponseTimeoutMax)
            {
                throw new ArgumentException($"TicketResponseTimeout must be less than {SdkInfo.TicketResponseTimeoutMax}ms");
            }
            if (ticketCancellationResponseTimeout < SdkInfo.TicketCancellationResponseTimeoutMin)
            {
                throw new ArgumentException($"TicketCancellationResponseTimeout must be more than {SdkInfo.TicketCancellationResponseTimeoutMin}ms");
            }
            if (ticketCancellationResponseTimeout > SdkInfo.TicketCancellationResponseTimeoutMax)
            {
                throw new ArgumentException($"TicketCancellationResponseTimeout must be less than {SdkInfo.TicketCancellationResponseTimeoutMax}ms");
            }
            if (ticketCashoutResponseTimeout < SdkInfo.TicketCashoutResponseTimeoutMin)
            {
                throw new ArgumentException($"TicketCashoutResponseTimeout must be more than {SdkInfo.TicketCashoutResponseTimeoutMin}ms");
            }
            if (ticketCashoutResponseTimeout > SdkInfo.TicketCashoutResponseTimeoutMax)
            {
                throw new ArgumentException($"TicketCashoutResponseTimeout must be less than {SdkInfo.TicketCashoutResponseTimeoutMax}ms");
            }

            Username = username;
            Password = password;
            Host = host;
            VirtualHost = string.IsNullOrEmpty(vhost)
                ? "/" + Username
                : vhost.StartsWith("/")
                    ? vhost
                    : "/" + vhost;
            UseSsl = useSsl;
            NodeId = nodeId > 0 ? nodeId : 1;
            BookmakerId = bookmakerId;
            LimitId = limitId;
            Currency = currency;
            Channel = channel;
            StatisticsEnabled = true;
            StatisticsRecordLimit = 1000000;
            StatisticsTimeout = 600;

            AccessToken = accessToken ?? string.Empty;
            ProvideAdditionalMarketSpecifiers = provideAdditionalMarketSpecifiers;

            Port = UseSsl ? 5671 : 5672;
            if (port > 0)
            {
                Port = port;
            }
            ExclusiveConsumer = exclusiveConsumer;

            if (Host.Contains(":"))
            {
                throw new ArgumentException("Host can not contain port number. Only domain name or ip address. E.g. mtsgate-ci.betradar.com");
            }

            KeycloakHost = keycloakHost;
            KeycloakUsername = keycloakUsername;
            KeycloakPassword = keycloakPassword;
            KeycloakSecret = keycloakSecret;
            MtsClientApiHost = mtsClientApiHost;

            if (MtsClientApiHost != null)
            {
                if (KeycloakHost == null)
                    throw new ArgumentException("KeycloakHost must be set.");
                if (KeycloakSecret == null)
                    throw new ArgumentException("KeycloakSecret must be set.");
            }

            TicketResponseTimeout = ticketResponseTimeout;
            TicketCancellationResponseTimeout = ticketCancellationResponseTimeout;
            TicketCashoutResponseTimeout = ticketCashoutResponseTimeout;
            TicketNonSrSettleResponseTimeout = ticketCashoutResponseTimeout;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdkConfiguration"/> class
        /// </summary>
        /// <param name="section">A <see cref="SdkConfigurationSection"/> instance containing config values</param>
        public SdkConfiguration(ISdkConfigurationSection section)
        {
            Contract.Requires(section != null);

            Username = section.Username;
            Password = section.Password;
            Host = section.Host;
            VirtualHost = string.IsNullOrEmpty(section.VirtualHost)
                ? "/" + Username
                : section.VirtualHost.StartsWith("/")
                    ? section.VirtualHost
                    : "/" + section.VirtualHost;
            UseSsl = section.UseSsl;
            NodeId = section.NodeId;
            BookmakerId = section.BookmakerId;
            LimitId = section.LimitId;
            Currency = section.Currency;
            Channel = section.Channel;

            StatisticsEnabled = section.StatisticsEnabled;
            StatisticsRecordLimit = section.StatisticsRecordLimit;
            StatisticsTimeout = section.StatisticsTimeout;
            AccessToken = section.AccessToken;
            ProvideAdditionalMarketSpecifiers = section.ProvideAdditionalMarketSpecifiers;
            Port = UseSsl ? 5671 : 5672;
            if (section.Port > 0)
            {
                Port = section.Port;
            }
            ExclusiveConsumer = section.ExclusiveConsumer;

            if (Host.Contains(":"))
            {
                throw new ArgumentException("Host can not contain port number. Only domain name or ip address. E.g. mtsgate-ci.betradar.com");
            }

            KeycloakHost = section.KeycloakHost;
            KeycloakUsername = section.KeycloakUsername;
            KeycloakPassword = section.KeycloakPassword;
            KeycloakSecret = section.KeycloakSecret;
            MtsClientApiHost = section.MtsClientApiHost;

            if (MtsClientApiHost != null)
            {
                if (KeycloakHost == null)
                    throw new ArgumentException("KeycloakHost must be set.");
                if (KeycloakSecret == null)
                    throw new ArgumentException("KeycloakSecret must be set.");
            }

            TicketResponseTimeout = section.TicketResponseTimeout;
            TicketCancellationResponseTimeout = section.TicketCancellationResponseTimeout;
            TicketCashoutResponseTimeout = section.TicketCashoutResponseTimeout;
        }

        /// <summary>
        /// Defined field invariants needed by code contracts
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(!string.IsNullOrEmpty(Username));
            Contract.Invariant(!string.IsNullOrEmpty(Password));
            Contract.Invariant(!string.IsNullOrEmpty(Host));
            Contract.Invariant(!string.IsNullOrEmpty(VirtualHost));
            Contract.Invariant(Port > 0);
            Contract.Invariant(!Host.Contains(":"), "Host can not contain port number. Only domain name or ip address. E.g. mtsgate-ci.betradar.com");
            Contract.Invariant(TicketResponseTimeout >= SdkInfo.TicketResponseTimeoutMin, $"TicketResponseTimeout must be more than {SdkInfo.TicketResponseTimeoutMin}ms");
            Contract.Invariant(TicketResponseTimeout <= SdkInfo.TicketResponseTimeoutMax, $"TicketResponseTimeout must be less than {SdkInfo.TicketResponseTimeoutMax}ms");
            Contract.Invariant(TicketCancellationResponseTimeout >= SdkInfo.TicketCancellationResponseTimeoutMin, $"TicketCancellationResponseTimeout must be more than {SdkInfo.TicketCancellationResponseTimeoutMin}ms");
            Contract.Invariant(TicketCancellationResponseTimeout <= SdkInfo.TicketCancellationResponseTimeoutMax, $"TicketCancellationResponseTimeout must be less than {SdkInfo.TicketCancellationResponseTimeoutMax}ms");
            Contract.Invariant(TicketCashoutResponseTimeout >= SdkInfo.TicketCashoutResponseTimeoutMin, $"TicketCashoutResponseTimeout must be more than {SdkInfo.TicketCashoutResponseTimeoutMin}ms");
            Contract.Invariant(TicketCashoutResponseTimeout <= SdkInfo.TicketCashoutResponseTimeoutMax, $"TicketCashoutResponseTimeout must be less than {SdkInfo.TicketCashoutResponseTimeoutMax}ms");
            Contract.Invariant(TicketNonSrSettleResponseTimeout <= SdkInfo.TicketCashoutResponseTimeoutMax, $"TicketCashoutResponseTimeout must be less than {SdkInfo.TicketCashoutResponseTimeoutMax}ms");
        }
    }
}