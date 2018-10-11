/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Internal.TicketImpl
{
    /// <summary>
    /// Implementation of <see cref="IBet"/>
    /// </summary>
    /// <seealso cref="IBet" />
    public class Bet : IBet
    {
        /// <summary>
        /// Gets the bonus of the bet (optional, default null)
        /// </summary>
        /// <value>The bonus</value>
        public IBetBonus Bonus { get; }
        /// <summary>
        /// Gets the stake of the bet
        /// </summary>
        /// <value>The stake</value>
        public IStake Stake { get; }
        /// <summary>
        /// Gets the id of the bet
        /// </summary>
        /// <value>The identifier</value>
        public string Id { get; }
        /// <summary>
        /// Gets array of all the systems (optional, if missing then complete accumulator is used)
        /// </summary>
        /// <value>The selected systems</value>
        public IEnumerable<int> SelectedSystems { get; }
        /// <summary>
        /// Gets the array of selection references which form the bet (optional, if missing then all selections are used)
        /// </summary>
        /// <value>The selection refs</value>
        public IEnumerable<ISelection> Selections { get; }
        /// <summary>
        /// Gets the reoffer reference bet id
        /// </summary>
        /// <value>The reoffer reference identifier</value>
        public string ReofferRefId { get; }
        /// <summary>
        /// Gets the sum of all wins for all generated combinations for this bet (in ticket currency, used in validation)
        /// </summary>
        /// <value>The sum of wins</value>
        public long SumOfWins { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bet"/> class
        /// </summary>
        /// <param name="bonus">The bonus</param>
        /// <param name="stake">The stake</param>
        /// <param name="betId">The bet identifier</param>
        /// <param name="selectedSystems">The selected systems</param>
        /// <param name="selections">The selections</param>
        /// <param name="reofferRefId">The reoffer reference identifier</param>
        /// <param name="sumOfWins">The sum of wins</param>
        public Bet(IBetBonus bonus, IStake stake, string betId, IEnumerable<int> selectedSystems, IEnumerable<ISelection> selections, string reofferRefId, long sumOfWins)
        {
            Contract.Requires(stake != null);
            Contract.Requires(betId == null || 
                               (Regex.IsMatch(betId, TicketHelper.IdPattern)
                               && betId.Length > 0
                               && betId.Length <= 128));
            Contract.Requires(selectedSystems == null
                              || (selectedSystems.Any()
                              && selectedSystems.Count() < 64
                              && selectedSystems.Count() == selectedSystems.Distinct().Count()
                              && selectedSystems.All(a => a > 0)));
            Contract.Requires(selections != null
                              && selections.Any()
                              && selections.Count() < 64
                              && selections.Count() == selections.Distinct().Count());
            Contract.Requires(string.IsNullOrEmpty(reofferRefId) || reofferRefId.Length <= 50);
            Contract.Requires(sumOfWins >= 0);

            Bonus = bonus;
            Stake = stake;
            Id = betId;
            SelectedSystems = selectedSystems;
            Selections = selections;
            ReofferRefId = reofferRefId;
            SumOfWins = sumOfWins;

            if (SelectedSystems != null)
            {
                var enumerable = SelectedSystems as IList<int> ?? SelectedSystems.ToList();
                if (SelectedSystems != null && enumerable.Any(a => a > Selections.Count()))
                {
                    throw new ArgumentException("Invalid value in SelectedSystems.");
                }
            }
        }

        /// <summary>
        /// Defines invariant members of the class
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(Stake != null);
            Contract.Invariant(Id == null ||
                              (Regex.IsMatch(Id, TicketHelper.IdPattern)
                               && Id.Length > 0
                               && Id.Length <= 128));
            Contract.Invariant(SelectedSystems == null
                              || (SelectedSystems.Any()
                                  && SelectedSystems.Count() < 64
                                  && SelectedSystems.Count() == SelectedSystems.Distinct().Count()
                                  && SelectedSystems.All(a => a > 0)));
            Contract.Invariant(Selections != null
                              && Selections.Any()
                              && Selections.Count() < 64
                              && Selections.Count() == Selections.Distinct().Count());
            Contract.Invariant(string.IsNullOrEmpty(ReofferRefId) || ReofferRefId.Length <= 50);
            Contract.Invariant(SumOfWins >= 0);
        }
    }
}