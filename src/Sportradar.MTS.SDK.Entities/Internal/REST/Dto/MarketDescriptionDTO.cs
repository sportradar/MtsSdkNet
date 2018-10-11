/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Sportradar.MTS.SDK.Entities.Internal.REST.Dto
{
    /// <summary>
    /// A data transfer object for market description
    /// </summary>
    public class MarketDescriptionDTO
    {
        public long Id { get; }

        internal string Name { get; }

        internal string Description { get; }

        internal string Variant { get; }

        internal IEnumerable<OutcomeDescriptionDTO> Outcomes { get; }

        internal IEnumerable<MarketSpecifierDTO> Specifiers { get; }

        internal IEnumerable<MarketMappingDTO> Mappings { get; }

        internal IEnumerable<MarketAttributeDTO> Attributes { get; }

        internal MarketDescriptionDTO(desc_market description)
        {
            Contract.Requires(description != null);
            Contract.Requires(!string.IsNullOrEmpty(description.name));

            Id = description.id;
            Name = description.name;
            Description = description.description;
            Outcomes = description.outcomes?.Select(o => new OutcomeDescriptionDTO(o)).ToList();
            Specifiers = description.specifiers?.Select(s => new MarketSpecifierDTO(s)).ToList();
            Mappings = description.mappings?.Select(m => new MarketMappingDTO(m)).ToList();
            Attributes = description.attributes?.Select(a => new MarketAttributeDTO(a)).ToList();
            Variant = description.variant;
        }
    }
}