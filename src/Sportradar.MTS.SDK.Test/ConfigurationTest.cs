/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sportradar.MTS.SDK.API;
using Sportradar.MTS.SDK.Entities.Enums;

namespace Sportradar.MTS.SDK.Test
{
    [TestClass]
    public class ConfigurationTest
    {
        private ISdkConfigurationBuilder _configurationBuilder;

        [TestInitialize]
        public void Init()
        {
            _configurationBuilder = MtsSdk.CreateConfigurationBuilder()
                                          .SetUsername("username")
                                          .SetPassword("password")
                                          .SetHost("127.0.0.1")
                                          .SetVirtualHost("/test")
                                          .SetUseSsl(true)
                                          .SetLimitId(111)
                                          .SetBookmakerId(222)
                                          .SetAccessToken("aaa")
                                          .SetCurrency("mBTC")
                                          .SetNode(10)
                                          .SetExclusiveConsumer(false)
                                          .SetSenderChannel(SenderChannel.Mobile)
                                          .SetMtsClientApiHost("127.0.0.1/ClientApi")
                                          .SetKeycloakHost("127.0.0.1/Keycloak")
                                          .SetKeycloakUsername("keycloak_username")
                                          .SetKeycloakPassword("keycloak_password")
                                          .SetKeycloakSecret("keycloak_secret");
        }

        [TestMethod]
        public void ConfigurationBuildTest()
        {
            var config = _configurationBuilder.Build();
            Assert.AreEqual("username", config.Username);
            Assert.AreEqual("password", config.Password);
            Assert.AreEqual("127.0.0.1", config.Host);
            Assert.AreEqual("/test", config.VirtualHost);
            Assert.AreEqual(true, config.UseSsl);
            Assert.AreEqual(111, config.LimitId);
            Assert.AreEqual(222, config.BookmakerId);
            Assert.AreEqual("aaa", config.AccessToken);
            Assert.AreEqual("mBTC", config.Currency);
            Assert.AreEqual(10, config.NodeId);
            Assert.AreEqual(SenderChannel.Mobile, config.Channel);
            Assert.AreEqual(5671, config.Port);
            Assert.AreEqual(false, config.ExclusiveConsumer);
            Assert.AreEqual("127.0.0.1/ClientApi", config.MtsClientApiHost);
            Assert.AreEqual("127.0.0.1/Keycloak", config.KeycloakHost);
            Assert.AreEqual("keycloak_username", config.KeycloakUsername);
            Assert.AreEqual("keycloak_password", config.KeycloakPassword);
            Assert.AreEqual("keycloak_secret", config.KeycloakSecret);
            Assert.AreEqual(15000, config.TicketResponseTimeout);
            Assert.AreEqual(600000, config.TicketCancellationResponseTimeout);
            Assert.AreEqual(600000, config.TicketCashoutResponseTimeout);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConfigurationBuilder_MissingUsername()
        {
            MtsSdk.CreateConfigurationBuilder()
                .SetPassword("test")
                .SetHost("127.0.0.1")
                .SetVirtualHost("/test")
                .SetUseSsl(false)
                .SetLimitId(111)
                .SetBookmakerId(222)
                .SetAccessToken("aaa")
                .SetCurrency("mBTC")
                .SetNode(10)
                .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConfigurationBuilder_MissingPassword()
        {
            MtsSdk.CreateConfigurationBuilder()
                .SetUsername("test")
                .SetHost("127.0.0.1")
                .SetVirtualHost("/test")
                .SetUseSsl(false)
                .SetLimitId(111)
                .SetBookmakerId(222)
                .SetAccessToken("aaa")
                .SetCurrency("mBTC")
                .SetNode(10)
                .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConfigurationBuilder_MissingHost()
        {
            MtsSdk.CreateConfigurationBuilder()
                .SetUsername("test")
                .SetPassword("test")
                .SetVirtualHost("/test")
                .SetUseSsl(false)
                .SetLimitId(111)
                .SetBookmakerId(222)
                .SetAccessToken("aaa")
                .SetCurrency("mBTC")
                .SetNode(10)
                .Build();
        }

        [TestMethod]
        public void ConfigurationBuilder_PortUpdates()
        {
            var config = MtsSdk.CreateConfigurationBuilder()
                .SetUsername("test")
                .SetPassword("test")
                .SetHost("127.0.0.1")
                .SetVirtualHost("/test")
                .SetUseSsl(false)
                .SetLimitId(111)
                .SetBookmakerId(222)
                .SetAccessToken("aaa")
                .SetCurrency("mBTC")
                .SetNode(10)
                .SetPort(111)
                .Build();

            Assert.IsNotNull(config);
            Assert.AreEqual(111, config.Port);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConfigurationBuilder_MissingKeycloakHost()
        {
            MtsSdk.CreateConfigurationBuilder()
                .SetUsername("test")
                .SetPassword("test")
                .SetHost("127.0.0.1")
                .SetVirtualHost("/test")
                .SetUseSsl(false)
                .SetLimitId(111)
                .SetBookmakerId(222)
                .SetAccessToken("aaa")
                .SetCurrency("mBTC")
                .SetNode(10)
                .SetPort(111)
                .SetMtsClientApiHost("127.0.0.1")
                .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConfigurationBuilder_MissingKeycloakSecret()
        {
            MtsSdk.CreateConfigurationBuilder()
                .SetUsername("test")
                .SetPassword("test")
                .SetHost("127.0.0.1")
                .SetVirtualHost("/test")
                .SetUseSsl(false)
                .SetLimitId(111)
                .SetBookmakerId(222)
                .SetAccessToken("aaa")
                .SetCurrency("mBTC")
                .SetNode(10)
                .SetPort(111)
                .SetMtsClientApiHost("127.0.0.1")
                .SetKeycloakHost("127.0.0.1")
                .Build();
        }

        [TestMethod]
        public void ConfigurationBuilder_AllowMissingKeycloakUsernameAndPassword()
        {
            MtsSdk.CreateConfigurationBuilder()
                .SetUsername("test")
                .SetPassword("test")
                .SetHost("127.0.0.1")
                .SetVirtualHost("/test")
                .SetUseSsl(false)
                .SetLimitId(111)
                .SetBookmakerId(222)
                .SetAccessToken("aaa")
                .SetCurrency("mBTC")
                .SetNode(10)
                .SetPort(111)
                .SetMtsClientApiHost("127.0.0.1")
                .SetKeycloakHost("127.0.0.1")
                .SetKeycloakSecret("secret")
                .Build();
        }
    }
}