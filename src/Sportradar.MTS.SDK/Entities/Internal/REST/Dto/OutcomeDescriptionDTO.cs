﻿/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using Dawn;

namespace Sportradar.MTS.SDK.Entities.Internal.REST.Dto
{
    /// <summary>
    /// A data-transfer-object for outcome description
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Approved")]
    internal class OutcomeDescriptionDTO
    {
        internal string Id { get; }

        internal string Name { get; }

        internal string Description { get; }

        internal OutcomeDescriptionDTO(desc_outcomesOutcome outcome)
        {
            Guard.Argument(outcome, nameof(outcome)).NotNull();
            Guard.Argument(outcome.id, nameof(outcome.id)).NotNull().NotEmpty();
            Guard.Argument(outcome.name, nameof(outcome.name)).NotNull().NotEmpty();

            Id = outcome.id;
            Name = outcome.name;
            Description = outcome.description;
        }
    }
}