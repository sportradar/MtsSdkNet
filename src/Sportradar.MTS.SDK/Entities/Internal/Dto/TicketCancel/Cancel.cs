﻿/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */

using System.Linq;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Internal.Dto.TicketCancel
{
    public partial class Cancel
    {
        public Cancel()
        { }

        //public Cancel(long timestamp, string ticketId, int bookmakerId, int code, string version)
        //{
        //    _timestampUtc = timestamp;
        //    _ticketId = ticketId;
        //    _code = code;
        //    _version = version;
        //    _sender = new Sender(bookmakerId);
        //}

        public Cancel(ITicketCancel ticket)
        {
            _timestampUtc = MtsTicketHelper.Convert(ticket.Timestamp);
            _ticketId = ticket.TicketId;
            _code = (int)ticket.Code;
            _version = ticket.Version;
            _sender = new Sender(ticket.BookmakerId);
            _cancelPercent = ticket.CancelPercent;
            _betCancel = ticket.BetCancels?.ToList().ConvertAll(c => new Anonymous(c));
        }
    }
}