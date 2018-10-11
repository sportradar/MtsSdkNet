/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using Microsoft.Practices.ObjectBuilder2;
using Sportradar.MTS.SDK.Entities.Internal;

namespace Sportradar.MTS.SDK.API.Internal.Senders
{
    public class TicketSenderFactory : ITicketSenderFactory
    {
        private readonly IReadOnlyDictionary<SdkTicketType, ITicketSender> _ticketSenders;

        private long _opened;

        public TicketSenderFactory(IReadOnlyDictionary<SdkTicketType, ITicketSender> senders)
        {
            Contract.Requires(senders != null);

            _ticketSenders = senders;
        }

        /// <summary>
        /// Defines invariant members of the class
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_ticketSenders != null);
            Contract.Invariant(_ticketSenders.Any());
        }

        public ITicketSender GetTicketSender(SdkTicketType ticketType)
        {
            ITicketSender sender;
            if (_ticketSenders.TryGetValue(ticketType, out sender))
            {
                Contract.Assume(sender != null);
                return sender;
            }
            return null;
        }

        public bool IsOpened => _opened == 1;

        public void Open()
        {
            if (Interlocked.CompareExchange(ref _opened, 1, 0) != 0)
            {
                throw new InvalidOperationException("The factory is already opened");
            }

            _ticketSenders.ForEach(f => f.Value.Open());
        }

        public void Close()
        {
            if (Interlocked.CompareExchange(ref _opened, 0, 1) != 1)
            {
                throw new InvalidOperationException("The factory is already closed");
            }

            _ticketSenders.ForEach(f => f.Value.Close());
        }
    }
}