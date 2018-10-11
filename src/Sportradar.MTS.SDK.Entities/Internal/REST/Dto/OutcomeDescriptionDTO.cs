/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;

namespace Sportradar.MTS.SDK.Entities.Internal.REST.Dto
{
    /// <summary>
    /// A data-transfer-object for outcome description
    /// </summary>
    public class OutcomeDescriptionDTO
    {
        internal string Id { get; }

        internal string Name { get; }

        internal string Description { get; }

        internal OutcomeDescriptionDTO(desc_outcomesOutcome outcome)
        {
            Contract.Requires(outcome != null);
            Contract.Requires(!string.IsNullOrEmpty(outcome.id));
            Contract.Requires(!string.IsNullOrEmpty(outcome.name));

            Id = outcome.id;
            Name = outcome.name;
            Description = outcome.description;
        }
    }
}