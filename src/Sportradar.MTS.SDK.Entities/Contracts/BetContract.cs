/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Sportradar.MTS.SDK.Entities.Interfaces;
using Sportradar.MTS.SDK.Entities.Internal;

namespace Sportradar.MTS.SDK.Entities.Contracts
{
    [ContractClassFor(typeof(IBet))]
    internal abstract class BetContract : IBet
    {
        public IBetBonus Bonus => Contract.Result<IBetBonus>();

        public IStake Stake
        {
            get
            {
                Contract.Ensures(Contract.Result<IStake>() != null);
                return Contract.Result<IStake>();
            }
        }

        public IStake EntireStake { get; }

        [Pure]
        public string Id
        {
            get
            {
                Contract.Ensures(string.IsNullOrEmpty(Contract.Result<string>()) || TicketHelper.ValidateTicketId(Contract.Result<string>()));
                return Contract.Result<string>();
            }
        }

        public IEnumerable<int> SelectedSystems
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<int>>() == null
                                 || (Contract.Result<IEnumerable<int>>().Any()
                                 && Contract.Result<IEnumerable<int>>().Count() < 64
                                 && Contract.Result<IEnumerable<int>>().Count() == Contract.Result<IEnumerable<int>>().Distinct().Count()
                                 && Contract.Result<IEnumerable<int>>().All(a => a > 0)));
                return Contract.Result<IEnumerable<int>>();
            }
        }

        public IEnumerable<ISelection> Selections {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<ISelection>>().Any()
                                 && Contract.Result<IEnumerable<ISelection>>().Count() < 64
                                 && Contract.Result<IEnumerable<ISelection>>().Count() == Contract.Result<IEnumerable<ISelection>>().Distinct().Count());
                return Contract.Result<IEnumerable<ISelection>>();
            }
        }

        public string ReofferRefId
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() == null
                                 || (Contract.Result<string>().Length >= 1
                                 && Contract.Result<string>().Length <= 50));
                return Contract.Result<string>();
            }
        }

        public long SumOfWins
        {
            get
            {
                Contract.Ensures(Contract.Result<long>() >= 0);
                return Contract.Result<long>();
            }
        }

        public bool? CustomBet { get; }
        public int? CalculationOdds { get; }
    }
}

