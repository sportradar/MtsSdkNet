/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sportradar.MTS.SDK.Entities.Enums;
using Sportradar.MTS.SDK.Entities.Internal.Builders;
using SR = Sportradar.MTS.SDK.Test.Helpers.StaticRandom;

namespace Sportradar.MTS.SDK.Test.Builders
{
    [TestClass]
    public class TicketCancelBuilderTest
    {
        [TestMethod]
        public void BuildBaseTicketTest()
        {
            var tb = TicketCancelBuilder.Create();
            var ticket = tb.SetTicketId("ticket-" + SR.I1000P).SetBookmakerId(SR.I1000).SetCode(TicketCancellationReason.BookmakerBackofficeTriggered).BuildTicket();

            Assert.IsNotNull(ticket);
            Assert.IsTrue(ticket.Timestamp > DateTime.Today.ToUniversalTime());
            Assert.AreEqual(ticket.Version, "2.0");
            Assert.IsTrue(!string.IsNullOrEmpty(ticket.TicketId));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void BuildTicketWithNoCodeTest()
        {
            var tb = TicketCancelBuilder.Create();
            var ticket = tb.SetTicketId("ticket-" + SR.I1000P).SetBookmakerId(SR.I1000).BuildTicket();

            Assert.IsNull(ticket);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void BuildTicketWithNoBookmakerIdTest()
        {
            var tb = TicketCancelBuilder.Create();
            var ticket = tb.SetTicketId("ticket-" + SR.I1000P).SetCode(TicketCancellationReason.BookmakerBackofficeTriggered).BuildTicket();

            Assert.IsNull(ticket);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void BuildTicketWithNoTicketIdTest()
        {
            var tb = TicketCancelBuilder.Create();
            var ticket = tb.SetBookmakerId(SR.I1000).SetCode(TicketCancellationReason.BookmakerBackofficeTriggered).BuildTicket();

            Assert.IsNull(ticket);
        }
    }
}