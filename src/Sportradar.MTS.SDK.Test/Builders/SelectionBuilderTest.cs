/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sportradar.MTS.SDK.Entities.Builders;
using Sportradar.MTS.SDK.Entities.Internal.Cache;
using Sportradar.MTS.SDK.Test.Helpers;
using SR = Sportradar.MTS.SDK.Test.Helpers.StaticRandom;

namespace Sportradar.MTS.SDK.Test.Builders
{
    [TestClass]
    [DeploymentItem("XML/market_descriptions.en.xml", "XML")]
    public class SelectionBuilderTest
    {
        private IMarketDescriptionProvider _marketDescriptionProvider;
        private ISelectionBuilder _selectionBuilder;

        [TestInitialize]
        public void Init()
        {
            var builderFactory = new BuilderFactoryHelper();
            _marketDescriptionProvider = builderFactory.MarketDescriptionProvider;
            _selectionBuilder = builderFactory.BuilderFactory.CreateSelectionBuilder();
            Assert.IsNotNull(_marketDescriptionProvider);
            Assert.IsNotNull(_selectionBuilder);
        }

        [TestMethod]
        public void SelectionBuilderGetMarketDescriptionTest()
        {
            var selection = _selectionBuilder.SetIdUof(1, "sr:sport:3", 282, "13", string.Empty, null).SetEventId(SR.I1000P).SetOdds(SR.I1000P).SetBanker(false).Build();
            Assert.IsNotNull(selection);
        }

        [TestMethod]
        public void SelectionBuilderSetIdUofFillsMarketDescriptionProviderTest()
        {
            var selection = _selectionBuilder.SetIdUof(1, "sr:sport:3", 282, "13", string.Empty, null).SetEventId(SR.I1000P).SetOdds(SR.I1000P).SetBanker(false).Build();

            Assert.IsNotNull(selection);

            Assert.IsNotNull(_marketDescriptionProvider);
            Assert.IsTrue(((MarketDescriptionCache)((MarketDescriptionProvider)_marketDescriptionProvider).MarketDescriptionCache).Cache.Any());
        }

        [TestMethod]
        public void SelectionBuilderSetIdUofWithNoSpecifiersMarketDescriptionProviderTest()
        {
            var selection = _selectionBuilder.SetIdUof(1, "sr:sport:1", 95, "74", (IReadOnlyDictionary<string, string>)null, null).SetEventId(SR.I1000P).SetOdds(SR.I1000P).SetBanker(false).Build();

            Assert.IsNotNull(selection);

            Assert.IsNotNull(_marketDescriptionProvider);
            Assert.IsTrue(((MarketDescriptionCache)((MarketDescriptionProvider)_marketDescriptionProvider).MarketDescriptionCache).Cache.Any());
        }

        [TestMethod]
        public void SelectionBuilderGetMarketDescriptionNonExistingTest()
        {
            var selection = _selectionBuilder.SetIdUof(1, "sr:sport:3", 28200, "13", string.Empty, null).SetEventId(SR.I1000P).SetOdds(SR.I1000P).SetBanker(false).Build();

            Assert.IsNotNull(selection);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void SelectionBuilderGetMarketDescriptionWithScoreButNoPropertiesTest()
        {
            var selection = _selectionBuilder.SetIdUof(1, "sr:sport:29", 62, "6", string.Empty, null).SetEventId(SR.I1000P).SetOdds(SR.I1000P).SetBanker(false).Build();

            Assert.IsNotNull(selection);
        }

        [TestMethod]
        public void SelectionBuilderGetMarketDescriptionWithScoreWithPropertiesTest()
        {
            var properties = new Dictionary<string, object> { { "HomeScore", "1" }, { "AwayScore", "1" } };
            var selection = _selectionBuilder.SetIdUof(1, "sr:sport:29", 62, "6", string.Empty, new ReadOnlyDictionary<string, object>(properties)).SetEventId(SR.I1000P).SetOdds(SR.I1000P).SetBanker(false).Build();

            Assert.IsNotNull(selection);
            Assert.IsTrue(selection.Id.Contains("$score=1:1"), "Selection id = " + selection.Id);
        }

        [TestMethod]
        public void SelectionBuilderGetMarketDescriptionWithScoreWithPropertiesCustomTest()
        {
            var properties = new Dictionary<string, object> { { "HomeScore", "1" }, { "AwayScore", "3" } };
            var selection = _selectionBuilder.SetIdUof(1, "sr:sport:1", 8, "6", "goalnr=4", new ReadOnlyDictionary<string, object>(properties)).SetEventId(SR.I1000P).SetOdds(SR.I1000P).SetBanker(false).Build();

            Assert.IsNotNull(selection);
            Assert.IsTrue(selection.Id.Contains("$score=1:3"), "Selection id = " + selection.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Missing argument exception")]
        public void SelectionBuilderGetMarketDescriptionWithScoreWithHomeScoreAndWithoutAwayScoreTest()
        {
            var properties = new Dictionary<string, object> { { "HomeScore", "1" } };
            var selection = _selectionBuilder.SetIdUof(1, "sr:sport:29", 62, "6", string.Empty, new ReadOnlyDictionary<string, object>(properties)).SetEventId(SR.I1000P).SetOdds(SR.I1000P).SetBanker(false).Build();

            Assert.IsNotNull(selection);
            Assert.IsTrue(selection.Id.Contains("$score=1:1"), "Selection id = " + selection.Id);
        }

        [TestMethod]
        public void SelectionBuilderGetMarketDescriptionWithScoreWithSpecifiersWithPropertiesTest()
        {
            var specifiers = new Dictionary<string, string> { { "total", "1" }, { "hcp", "0.25" } };
            var properties = new Dictionary<string, object> { { "HomeScore", "1" }, { "AwayScore", "1" } };
            var selection = _selectionBuilder.SetIdUof(1, "sr:sport:29", 62, "6", new ReadOnlyDictionary<string, string>(specifiers), new ReadOnlyDictionary<string, object>(properties)).SetEventId(SR.I1000P).SetOdds(SR.I1000P).SetBanker(false).Build();

            Assert.IsNotNull(selection);
            Assert.IsTrue(selection.Id.Contains("total=1"), "Selection id = " + selection.Id);
            Assert.IsTrue(selection.Id.Contains("hcp=0.25"), "Selection id = " + selection.Id);
            Assert.IsTrue(selection.Id.Contains("$score=1:1"), "Selection id = " + selection.Id);
        }

        [TestMethod]
        public void SelectionBuilderGetMarketDescriptionForMarket215WithServerTest()
        {
            var properties = new Dictionary<string, object> { { "HomeScore", "1" }, { "AwayScore", "1" }, { "CurrentServer", "1" } };
            var selection = _selectionBuilder.SetIdUof(1, "sr:sport:29", 215, "6", string.Empty, new ReadOnlyDictionary<string, object>(properties)).SetEventId(SR.I1000P).SetOdds(SR.I1000P).SetBanker(false).Build();

            Assert.IsNotNull(selection);
            Assert.IsTrue(selection.Id.Contains("$server=1"), "Selection id = " + selection.Id);
        }

        [TestMethod]
        public void SelectionBuilderGetMarketDescriptionForMarket215WithServerHasStringSpecifiersTest()
        {
            var properties = new Dictionary<string, object> { { "HomeScore", "1" }, { "AwayScore", "1" }, { "CurrentServer", "1" } };
            var selection = _selectionBuilder.SetIdUof(1, "sr:sport:29", 215, "6", "total=1|hcp=0.25", new ReadOnlyDictionary<string, object>(properties)).SetEventId(SR.I1000P).SetOdds(SR.I1000P).SetBanker(false).Build();

            Assert.IsNotNull(selection);
            Assert.IsTrue(selection.Id.Contains("total=1"), "Selection id = " + selection.Id);
            Assert.IsTrue(selection.Id.Contains("hcp=0.25"), "Selection id = " + selection.Id);
            Assert.IsTrue(selection.Id.Contains("$server=1"), "Selection id = " + selection.Id);
        }

        [TestMethod]
        public void SelectionBuilderGetMarketDescriptionForMarket215WithServerHasArraySpecifiersTest()
        {
            var specifiers = new Dictionary<string, string> { { "total", "1" }, { "hcp", "0.25" } };
            var properties = new Dictionary<string, object> { { "HomeScore", "1" }, { "AwayScore", "1" }, { "CurrentServer", "1" } };
            var selection = _selectionBuilder.SetIdUof(1, "sr:sport:29", 215, "6", new ReadOnlyDictionary<string, string>(specifiers), new ReadOnlyDictionary<string, object>(properties)).SetEventId(SR.I1000P).SetOdds(SR.I1000P).SetBanker(false).Build();

            Assert.IsNotNull(selection);
            Assert.IsTrue(selection.Id.Contains("total=1"), "Selection id = " + selection.Id);
            Assert.IsTrue(selection.Id.Contains("hcp=0.25"), "Selection id = " + selection.Id);
            Assert.IsTrue(selection.Id.Contains("$server=1"), "Selection id = " + selection.Id);
        }

        [TestMethod]
        public void SelectionBuilderGetMarketDescriptionForMarket215WithEmptySpecifiersTest()
        {
            var specifiers = new Dictionary<string, string>();
            var properties = new Dictionary<string, object> { { "HomeScore", "1" }, { "AwayScore", "1" }, { "CurrentServer", "1" } };
            var selection = _selectionBuilder.SetIdUof(1, "sr:sport:29", 215, "6", new ReadOnlyDictionary<string, string>(specifiers), new ReadOnlyDictionary<string, object>(properties)).SetEventId(SR.I1000P).SetOdds(SR.I1000P).SetBanker(false).Build();

            Assert.IsNotNull(selection);
            Assert.IsTrue(selection.Id.Contains("$server=1"), "Selection id = " + selection.Id);
            Assert.AreEqual("$server=1", selection.Id.Substring(selection.Id.IndexOf("?", StringComparison.InvariantCultureIgnoreCase)+1));
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void SelectionBuilderGetMarketDescriptionForMarket215WithNoServerTest()
        {
            var properties = new Dictionary<string, object> { { "HomeScore", "1" }, { "AwayScore", "1" } };
            var selection = _selectionBuilder.SetIdUof(1, "sr:sport:29", 215, "6", string.Empty, new ReadOnlyDictionary<string, object>(properties)).SetEventId(SR.I1000P).SetOdds(SR.I1000P).SetBanker(false).Build();

            Assert.IsNotNull(selection);
            Assert.IsTrue(selection.Id.Contains("$server=1"), "Selection id = " + selection.Id);
        }
    }
}