﻿/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections.Generic;
using Dawn;
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
            Guard.Argument(senders).NotNull();

            _ticketSenders = senders;
        }

        public ITicketSender GetTicketSender(SdkTicketType ticketType)
        {
            if (_ticketSenders.TryGetValue(ticketType, out var sender))
            {
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