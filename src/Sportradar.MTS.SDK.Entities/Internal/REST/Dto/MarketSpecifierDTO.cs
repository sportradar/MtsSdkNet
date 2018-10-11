/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;

namespace Sportradar.MTS.SDK.Entities.Internal.REST.Dto
{
    /// <summary>
    /// A data-transfer-object representation for specifier
    /// </summary>
    public class MarketSpecifierDTO
    {
        internal string Name { get; }

        internal string Type { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MarketSpecifierDTO"/> class
        /// </summary>
        /// <param name="specifier">The <see cref="desc_specifiersSpecifier"/> used for creating instance</param>
        internal MarketSpecifierDTO(desc_specifiersSpecifier specifier)
        {
            Contract.Requires(specifier != null);
            Contract.Requires(!string.IsNullOrEmpty(specifier.name));
            Contract.Requires(!string.IsNullOrEmpty(specifier.type));


            Name = specifier.name;
            Type = specifier.type;
        }
    }
}