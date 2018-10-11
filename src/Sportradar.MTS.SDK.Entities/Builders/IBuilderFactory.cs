/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Contracts;

namespace Sportradar.MTS.SDK.Entities.Builders
{
    /// <summary>
    /// Defines a contract used to construct builder instances used when constructing tickets and it's associated entities
    /// </summary>
    [ContractClass(typeof(BuilderFactoryContract))]
    public interface IBuilderFactory : ISimpleBuilderFactory
    {
        /// <summary>
        /// Constructs and returns a new instance of the <see cref="ITicketCancelBuilder"/> class
        /// </summary>
        /// <returns>A new instance of the <see cref="ITicketCancelBuilder"/> class</returns>
        ITicketCancelBuilder CreateTicketCancelBuilder();

        /// <summary>
        /// Constructs and returns a new instance of the <see cref="ITicketReofferCancelBuilder"/> class
        /// </summary>
        /// <returns>A new instance of the <see cref="ITicketReofferCancelBuilder"/> class</returns>
        ITicketReofferCancelBuilder CreateTicketReofferCancelBuilder();

        /// <summary>
        /// Constructs and returns a new instance of the <see cref="ITicketCashoutBuilder"/> class
        /// </summary>
        /// <returns>A new instance of the <see cref="ITicketCashoutBuilder"/> class</returns>
        ITicketCashoutBuilder CreateTicketCashoutBuilder();

        /// <summary>
        /// Constructs and returns a new instance of the <see cref="ISenderBuilder"/> class
        /// </summary>
        /// <returns>A new instance of the <see cref="ISenderBuilder"/> class</returns>
        ISenderBuilder CreateSenderBuilder();

        /// <summary>
        /// Constructs and returns a new instance of the <see cref="ISelectionBuilder"/> class
        /// </summary>
        /// <returns>A new instance of the <see cref="ISelectionBuilder"/> class</returns>
        ISelectionBuilder CreateSelectionBuilder();

        /// <summary>
        /// Constructs and returns a new instance of the <see cref="ITicketAckBuilder"/> class
        /// </summary>
        /// <returns>A new instance of the <see cref="ITicketAckBuilder"/> class</returns>
        ITicketAckBuilder CreateTicketAckBuilder();

        /// <summary>
        /// Constructs and returns a new instance of the <see cref="ITicketCancelAckBuilder"/> class
        /// </summary>
        /// <returns>A new instance of the <see cref="ITicketCancelAckBuilder"/> class</returns>
        ITicketCancelAckBuilder CreateTicketCancelAckBuilder();
    }
}
