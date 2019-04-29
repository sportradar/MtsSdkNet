﻿/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Collections.Generic;
using System.Linq;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Internal.Dto.Ticket
{
    /// <summary>
    /// Class for BET
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public partial class Anonymous
    {
        public Anonymous()
        { }

        public Anonymous(string id, long sumOfWins, Stake stake, Bonus bonus, IEnumerable<int> selectedSystems, IEnumerable<Anonymous3> selectionRefs, string reofferRefId)
        {
            _id = string.IsNullOrEmpty(id) ? null : id;
            _sumOfWins = sumOfWins > 0 ? sumOfWins : (long?)null;
            _stake = stake;
            _bonus = null;
            if (bonus != null)
            {
                _bonus = bonus;
            }
            _selectedSystems = null;
            if (selectedSystems != null)
            {
                _selectedSystems = selectedSystems as IReadOnlyCollection<int>;
            }
            _selectionRefs = null;
            if (selectionRefs != null)
            {
                _selectionRefs = selectionRefs as IReadOnlyCollection<Anonymous3>;
            }
            _reofferRefId = string.IsNullOrEmpty(reofferRefId) ? null : reofferRefId;

            _customBet = null;
            _calculationOdds = null;
            _entireStake = null;
        }

        public Anonymous(IBet bet, IEnumerable<ISelectionRef> selectionRefs)
        {
            _id = string.IsNullOrEmpty(bet.Id) ? null : bet.Id;
            _sumOfWins = bet.SumOfWins > 0 ? bet.SumOfWins : (long?)null;
            _stake = new Stake(bet.Stake);
            _bonus = null;
            if (bet.Bonus != null)
            {
                _bonus = new Bonus(bet.Bonus);
            }
            _selectedSystems = null;
            if (bet.SelectedSystems != null)
            {
                _selectedSystems = bet.SelectedSystems as IReadOnlyCollection<int>;
            }
            _selectionRefs = null;
            if (selectionRefs != null)
            {
                _selectionRefs = selectionRefs.ToList().ConvertAll(b => new Anonymous3(b));
            }

            _customBet = null;
            _calculationOdds = null;
            _entireStake = null;
        }
    }
}