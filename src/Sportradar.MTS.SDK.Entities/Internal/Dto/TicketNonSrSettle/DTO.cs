using Sportradar.MTS.SDK.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sportradar.MTS.SDK.Entities.Internal.Dto.TicketNonSrSettle
{
    public partial class TicketNonSrSettleDTO
    {
        public TicketNonSrSettleDTO()
        { }

        public TicketNonSrSettleDTO(ITicketNonSrSettle ticket)
        {
            _timestampUtc = MtsTicketHelper.Convert(ticket.Timestamp);
            _ticketId = ticket.TicketId;
            _nonSrSettleStake = ticket.NonSrSettleStake;
            _version = ticket.Version;
            _sender = new Sender(ticket.BookmakerId);
        }
    }
}
