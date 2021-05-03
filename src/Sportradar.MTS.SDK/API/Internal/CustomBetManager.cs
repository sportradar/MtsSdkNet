﻿/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using log4net;
using Sportradar.MTS.SDK.Common.Exceptions;
using Sportradar.MTS.SDK.Common.Internal;
using Sportradar.MTS.SDK.Common.Log;
using Sportradar.MTS.SDK.Entities.Builders;
using Sportradar.MTS.SDK.Entities.Interfaces.CustomBet;
using Sportradar.MTS.SDK.Entities.Internal;
using Sportradar.MTS.SDK.Entities.Internal.Builders;
using Sportradar.MTS.SDK.Entities.Internal.CustomBetImpl;
using Sportradar.MTS.SDK.Entities.Internal.Dto.CustomBet;

namespace Sportradar.MTS.SDK.API.Internal
{
    /// <summary>
    /// The run-time implementation of the <see cref="ICustomBetManager"/> interface
    /// </summary>
    internal class CustomBetManager : ICustomBetManager
    {
        private readonly ILog _clientLog = SdkLoggerFactory.GetLoggerForClientInteraction(typeof(CustomBetManager));
        private readonly ILog _executionLog = SdkLoggerFactory.GetLoggerForExecution(typeof(CustomBetManager));

        private readonly IDataProvider<AvailableSelectionsDTO> _availableSelectionsProvider;
        private readonly ICalculateProbabilityProvider _calculateProbabilityProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomBetManager"/> class
        /// </summary>
        /// <param name="availableSelectionsProvider">A <see cref="IDataProvider{AvailableSelectionsDTO}"/> used to make custom bet API requests</param>
        /// <param name="calculateProbabilityProvider">A <see cref="ICalculateProbabilityProvider"/> used to make custom bet API requests</param>
        public CustomBetManager(IDataProvider<AvailableSelectionsDTO> availableSelectionsProvider, ICalculateProbabilityProvider calculateProbabilityProvider)
        {
            _availableSelectionsProvider = availableSelectionsProvider ?? throw new ArgumentNullException(nameof(availableSelectionsProvider));
            _calculateProbabilityProvider = calculateProbabilityProvider ?? throw new ArgumentNullException(nameof(calculateProbabilityProvider));
        }

        public async Task<IAvailableSelections> GetAvailableSelectionsAsync(string eventId)
        {
            CheckMethodArguments(eventId);

            try
            {
                _clientLog.Info($"Invoking CustomBetManager.GetAvailableSelectionsAsync({eventId})");
                var availableSelections = await _availableSelectionsProvider.GetDataAsync(eventId).ConfigureAwait(false);
                return new AvailableSelections(availableSelections);
            }
            catch (CommunicationException ce)
            {
                _executionLog.Warn($"Event[{eventId}] getting available selections failed, CommunicationException: {ce.Message}");
                throw;
            }
            catch (Exception e)
            {
                _executionLog.Warn($"Event[{eventId}] getting available selections failed.", e);
                throw;
            }
        }

        public async Task<ICalculation> CalculateProbabilityAsync(IEnumerable<ISelection> selections)
        {
            var selectionsList = selections as IReadOnlyCollection<ISelection>;
            
            CheckMethodArguments(selectionsList);

            try
            {
                _clientLog.Info($"Invoking CustomBetManager.CalculateProbability({selections})");
                var calculationResponse = await _calculateProbabilityProvider.GetDataAsync(selections).ConfigureAwait(false);
                return new Calculation(calculationResponse);
            }
            catch (CommunicationException ce)
            {
                _executionLog.Warn($"Calculating probabilities failed, CommunicationException: {ce.Message}");
                throw;
            }
            catch (Exception e)
            {
                _executionLog.Warn($"Calculating probabilities failed.", e);
                throw;
            }
        }

        private void CheckMethodArguments(string eventId)
        {
            if (eventId.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(eventId));
            }
        }

        private void CheckMethodArguments(IEnumerable<ISelection> selections)
        {
            if (selections.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(selections));
            }
        }

        public ICustomBetSelectionBuilder CustomBetSelectionBuilder => new CustomBetSelectionBuilder();
    }
}
