﻿/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections.Generic;
using System.Linq;
using Sportradar.MTS.SDK.Entities.Internal.REST;

namespace Sportradar.MTS.SDK.Entities.Internal.Dto.CustomBet
{
    /// <summary>
    /// Defines a data-transfer-object for available selections for the market
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Approved")]
    internal class MarketDTO
    {
        /// <summary>
        /// Gets the id of the market
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the specifiers for this market
        /// </summary>
        public string Specifiers { get; }

        /// <summary>
        /// Gets the outcomes for this market
        /// </summary>
        public IEnumerable<string> Outcomes { get; }

        internal MarketDTO(MarketType market)
        {
            if (market == null)
                throw new ArgumentNullException(nameof(market));

            Id = market.id;
            Specifiers = market.specifiers;
            Outcomes = market.outcome.Select(o => o.id).ToList().AsReadOnly();
        }
    }
}
