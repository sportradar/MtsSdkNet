/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Builders;

namespace Sportradar.MTS.SDK.Entities.Contracts
{
    [ContractClassFor(typeof(ISimpleBuilderFactory))]
    internal abstract class SimpleBuilderFactoryContract : ISimpleBuilderFactory
    {
        public ITicketBuilder CreateTicketBuilder()
        {
            Contract.Ensures(Contract.Result<ITicketBuilder>() != null);
            return Contract.Result<ITicketBuilder>();
        }

        public ITicketReofferBuilder CreateTicketReofferBuilder()
        {
            Contract.Ensures(Contract.Result<ITicketReofferBuilder>() != null);
            return Contract.Result<ITicketReofferBuilder>();
        }

        public IEndCustomerBuilder CreateEndCustomerBuilder()
        {
            Contract.Ensures(Contract.Result<IEndCustomerBuilder>() != null);
            return Contract.Result<IEndCustomerBuilder>();
        }

        public IBetBuilder CreateBetBuilder()
        {
            Contract.Ensures(Contract.Result<IBetBuilder>() != null);
            return Contract.Result<IBetBuilder>();
        }

        public ITicketAltStakeBuilder CreateAltStakeBuilder()
        {
            Contract.Ensures(Contract.Result<ITicketAltStakeBuilder>() != null);
            return Contract.Result<ITicketAltStakeBuilder>();
        }
    }
}