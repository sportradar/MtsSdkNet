/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Serialization;
using log4net;
using Sportradar.MTS.SDK.Common.Exceptions;
using Sportradar.MTS.SDK.Common.Log;
using Sportradar.MTS.SDK.Entities.Builders;
using Sportradar.MTS.SDK.Entities.Interfaces.CustomBet;
using Sportradar.MTS.SDK.Entities.Internal;
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
        private readonly ICustomBetSelectionBuilder _customBetSelectionBuilder;
        private readonly XmlSerializer _serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomBetManager"/> class
        /// </summary>
        /// <param name="availableSelectionsProvider">A <see cref="IDataProvider{AvailableSelectionsDTO}"/> used to make custom bet API requests</param>
        /// <param name="calculationResponseProvider">A <see cref="ICalculateProbabilityProvider"/> used to make custom bet API requests</param>
        /// <param name="customBetSelectionBuilder">A <see cref="ICustomBetSelectionBuilder"/> used to build selections</param>
        public CustomBetManager(IDataProvider<AvailableSelectionsDTO> availableSelectionsProvider, ICalculateProbabilityProvider calculateProbabilityProvider, ICustomBetSelectionBuilder customBetSelectionBuilder)
        {
            if (availableSelectionsProvider == null)
                throw new ArgumentNullException(nameof(availableSelectionsProvider));
            if (calculateProbabilityProvider == null)
                throw new ArgumentNullException(nameof(calculateProbabilityProvider));
            if (customBetSelectionBuilder == null)
                throw new ArgumentNullException(nameof(customBetSelectionBuilder));

            _availableSelectionsProvider = availableSelectionsProvider;
            _calculateProbabilityProvider = calculateProbabilityProvider;
            CustomBetSelectionBuilder = customBetSelectionBuilder;
        }

        public async Task<IAvailableSelections> GetAvailableSelectionsAsync(string eventId)
        {
            if (eventId == null)
                throw new ArgumentNullException(nameof(eventId));

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
            if (selections == null)
                throw new ArgumentNullException(nameof(selections));

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

        public ICustomBetSelectionBuilder CustomBetSelectionBuilder { get; }
    }
}
