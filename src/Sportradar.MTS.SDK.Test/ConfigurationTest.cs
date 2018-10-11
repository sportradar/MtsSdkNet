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
                                          .SetSenderChannel(SenderChannel.Mobile);
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
    }
}