/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Builders;

namespace Sportradar.MTS.SDK.Entities.Contracts
{
    [ContractClassFor(typeof(IBuilderFactory))]
    internal abstract class BuilderFactoryContract : IBuilderFactory
    {
        public abstract ITicketBuilder CreateTicketBuilder();

        public abstract ITicketReofferBuilder CreateTicketReofferBuilder();

        public abstract IEndCustomerBuilder CreateEndCustomerBuilder();

        public abstract IBetBuilder CreateBetBuilder();
        
        public abstract ITicketAltStakeBuilder CreateAltStakeBuilder();

        public ITicketCancelBuilder CreateTicketCancelBuilder()
        {
            Contract.Ensures(Contract.Result<ITicketCancelBuilder>() != null);
            return Contract.Result<ITicketCancelBuilder>();
        }

        public ITicketReofferCancelBuilder CreateTicketReofferCancelBuilder()
        {
            Contract.Ensures(Contract.Result<ITicketReofferCancelBuilder>() != null);
            return Contract.Result<ITicketReofferCancelBuilder>();
        }

        public ITicketCashoutBuilder CreateTicketCashoutBuilder()
        {
            Contract.Ensures(Contract.Result<ITicketCashoutBuilder>() != null);
            return Contract.Result<ITicketCashoutBuilder>();
        }

        public ISenderBuilder CreateSenderBuilder()
        {
            Contract.Ensures(Contract.Result<ISenderBuilder>() != null);
            return Contract.Result<ISenderBuilder>();
        }

        public ISelectionBuilder CreateSelectionBuilder()
        {
            Contract.Ensures(Contract.Result<ISelectionBuilder>() != null);
            return Contract.Result<ISelectionBuilder>();
        }

        public ITicketAckBuilder CreateTicketAckBuilder()
        {
            Contract.Ensures(Contract.Result<ITicketAckBuilder>() != null);
            return Contract.Result<ITicketAckBuilder>();
        }

        public ITicketCancelAckBuilder CreateTicketCancelAckBuilder()
        {
            Contract.Ensures(Contract.Result<ITicketCancelAckBuilder>() != null);
            return Contract.Result<ITicketCancelAckBuilder>();
        }
    }
}