/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Enums;
using Sportradar.MTS.SDK.Entities.Interfaces;
// ReSharper disable UnusedMember.Local
// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local

namespace Sportradar.MTS.SDK.Entities.Internal.TicketImpl
{
    public class Sender : ISender
    {
        /// <summary>
        /// Gets the ticket bookmaker id (client's id provided by Sportradar)
        /// </summary>
        public int BookmakerId { get; }

        /// <summary>
        /// Gets the 3 letter currency code
        /// </summary>
        public string Currency { get; }


        /// <summary>
        /// Gets the terminal id
        /// </summary>
        public string TerminalId { get; }

        /// <summary>
        /// Gets the senders communication channel
        /// </summary>
        public SenderChannel Channel { get; }

        /// <summary>
        /// Gets the shop id
        /// </summary>
        public string ShopId { get; }


        /// <summary>
        /// Gets the identification of the end user (customer)
        /// </summary>
        public IEndCustomer EndCustomer { get; }


        /// <summary>
        /// Gets the client's limit id (provided by Sportradar to the client)
        /// </summary>
        public int LimitId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sender"/> class
        /// </summary>
        /// <param name="bookmakerId">The bookmaker identifier</param>
        /// <param name="currency">The currency</param>
        /// <param name="terminalId">The terminal identifier</param>
        /// <param name="senderChannel">The sender channel</param>
        /// <param name="shopId">The shop identifier</param>
        /// <param name="customer">The customer</param>
        /// <param name="limitId">The limit identifier</param>
        public Sender(int bookmakerId, 
                      string currency, 
                      string terminalId, 
                      SenderChannel senderChannel, 
                      string shopId, 
                      IEndCustomer customer, 
                      int limitId)
        {
            Contract.Requires(bookmakerId > 0);
            Contract.Requires(!string.IsNullOrEmpty(currency));
            Contract.Requires(currency.Length == 3 || currency.Length == 4);
            Contract.Requires(string.IsNullOrEmpty(terminalId) || TicketHelper.ValidStringId(terminalId, true, 1, 36));
            Contract.Requires(string.IsNullOrEmpty(shopId) || TicketHelper.ValidStringId(shopId, true, 1, 36));
            Contract.Requires(limitId > 0);


            BookmakerId = bookmakerId;
            Currency = currency.Length == 3 ? currency.ToUpper() : currency;
            TerminalId = terminalId;
            Channel = senderChannel;
            ShopId = shopId;
            EndCustomer = customer;
            LimitId = limitId;

            ValidateSenderData();
        }

        /// <summary>
        /// Defines invariant members of the class
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(BookmakerId > 0);
            Contract.Invariant(!string.IsNullOrEmpty(Currency) && Currency.Length >= 3 && Currency.Length <= 4);
            Contract.Invariant(string.IsNullOrEmpty(TerminalId) || TicketHelper.ValidStringId(TerminalId, true, 1, 36));
            Contract.Invariant(string.IsNullOrEmpty(ShopId) || TicketHelper.ValidStringId(ShopId, true, 1, 36));
            Contract.Invariant(LimitId > 0);
        }

        private void ValidateSenderData()
        {
            CheckArgument(BookmakerId > 0, "BookmakerId", "BookmakerId is invalid.");
            CheckArgument(LimitId > 0, "LimitId", "LimitId is invalid.");
            CheckArgument(!string.IsNullOrEmpty(Currency), "Currency", "Currency is invalid.");
        }

        // ReSharper disable once UnusedParameter.Local
        private static void CheckNotNull(object input, string paramName, string msg)
        {
            if (input == null)
            {
                throw new ArgumentNullException(paramName, msg);
            }
        }

        // ReSharper disable once UnusedParameter.Local
        private static void CheckArgument(bool input, string paramName, string msg)
        {
            if (!input)
            {
                throw new ArgumentException(msg, paramName);
            }
        }
    }
}