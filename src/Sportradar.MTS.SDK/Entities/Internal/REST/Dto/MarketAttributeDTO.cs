﻿/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using Dawn;

namespace Sportradar.MTS.SDK.Entities.Internal.REST.Dto
{
    /// <summary>
    /// A data-transfer-object representing a market description attributes
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Approved")]
    internal class MarketAttributeDTO
    {
        /// <summary>
        /// Gets the attribute name
        /// </summary>
        /// <value>The name</value>
        public string Name { get; }

        /// <summary>
        /// Gets the attribute description
        /// </summary>
        /// <value>The description</value>
        public string Description { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MarketAttributeDTO"/> class
        /// </summary>
        /// <param name="record">A <see cref="attributesAttribute"/> representing attribute object obtained by parsing the xml</param>
        public MarketAttributeDTO(attributesAttribute record)
        {
            Guard.Argument(record, nameof(record)).NotNull();

            Name = record.name;
            Description = record.description;
        }
    }
}