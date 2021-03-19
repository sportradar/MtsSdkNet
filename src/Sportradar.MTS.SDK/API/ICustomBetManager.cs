﻿/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportradar.MTS.SDK.Entities.Builders;
using Sportradar.MTS.SDK.Entities.Interfaces.CustomBet;

namespace Sportradar.MTS.SDK.API
{
    /// <summary>
    /// Defines methods used to perform various custom bet operations
    /// </summary>
    public interface ICustomBetManager
    {
        /// <summary>
        /// Returns an <see cref="IAvailableSelections"/> instance providing the available selections for the event associated with the provided <see cref="string"/> identifier
        /// </summary>
        /// <param name="eventId">The <see cref="string"/> identifier of the event for which the available selections should be returned</param>
        /// <returns>An <see cref="IAvailableSelections"/> providing the the available selections of the associated event</returns>
        Task<IAvailableSelections> GetAvailableSelectionsAsync(string eventId);

        /// <summary>
        /// Returns an <see cref="ICalculation"/> instance providing the probability for the specified selections
        /// </summary>
        /// <param name="selections">The <see cref="IEnumerable{ISelection}"/> containing selections for which the probability should be calculated</param>
        /// <returns>An <see cref="ICalculation"/> providing the the probability for the specified selections</returns>
        Task<ICalculation> CalculateProbabilityAsync(IEnumerable<ISelection> selections);

        /// <summary>
        /// Returns an <see cref="ICustomBetSelectionBuilder"/> instance used to build selections
        /// </summary>
        /// <returns>An <see cref="ICustomBetSelectionBuilder"/> instance used to build selections</returns>
        ICustomBetSelectionBuilder CustomBetSelectionBuilder { get; }
    }
}
