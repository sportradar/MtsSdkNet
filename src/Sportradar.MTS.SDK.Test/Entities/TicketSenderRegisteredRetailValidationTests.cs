﻿/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sportradar.MTS.SDK.Entities.Builders;
using Sportradar.MTS.SDK.Entities.Enums;
using Sportradar.MTS.SDK.Entities.Internal.Builders;
using Sportradar.MTS.SDK.Test.Helpers;

namespace Sportradar.MTS.SDK.Test.Entities
{
    [TestClass]
    public class TicketSenderRegisteredRetailValidationTests
    {
        [TestMethod]
        public void limit_is_required()
        {
            var builder = SenderBuilder.Create()
                .SetSenderChannel(SenderChannel.Retail)
                .SetShopId("shop")
                .SetBookmakerId(1)
                .SetCurrency("eur")
                .SetEndCustomer(EndCustomerBuilder.Create().SetId("id").SetLanguageId("en").Build());

            try
            {
                builder.Build();
                Assert.Fail("Build should throw an exception");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual(TestHelper.ChannelParamName, ex.ParamName, "Argument exception for wrong argument was thrown");
            }
        }

        [TestMethod]
        public void end_customer_id_is_not_required_for_retail()
        {
            var builder = SenderBuilder.Create()
                .SetSenderChannel(SenderChannel.Retail)
                .SetLimitId(1)
                .SetBookmakerId(1)
                .SetShopId("a")
                .SetCurrency("eur")
                .SetEndCustomer(EndCustomerBuilder.Create().SetLanguageId("en").Build());

            var sender = builder.Build();
            Assert.IsNotNull(sender);
        }

        [TestMethod]
        public void end_customer_device_id_is_allowed()
        {
            var builder = SenderBuilder.Create()
                .SetSenderChannel(SenderChannel.Retail)
                .SetLimitId(1)
                .SetBookmakerId(1)
                .SetShopId("a")
                .SetCurrency("eur")
                .SetEndCustomer(EndCustomerBuilder.Create().SetId("id").SetDeviceId("device").SetLanguageId("en").Build());

            var sender = builder.Build();
            Assert.IsNotNull(sender);
        }

        [TestMethod]
        public void valid_sender_is_validated()
        {
            var builder = SenderBuilder.Create()
                .SetSenderChannel(SenderChannel.Retail)
                .SetLimitId(1)
                .SetBookmakerId(1)
                .SetShopId("a")
                .SetCurrency("eur")
                .SetEndCustomer(EndCustomerBuilder.Create().SetId("id").SetLanguageId("en").Build());

            var sender = builder.Build();
            Assert.IsNotNull(sender);
        }
    }
}