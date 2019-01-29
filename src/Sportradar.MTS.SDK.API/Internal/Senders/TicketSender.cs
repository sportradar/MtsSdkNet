/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.API.Internal.Mappers;
using Sportradar.MTS.SDK.API.Internal.RabbitMq;
using Sportradar.MTS.SDK.Entities.Interfaces;
using Sportradar.MTS.SDK.Entities.Internal;
using Sportradar.MTS.SDK.Entities.Internal.Dto.Ticket;

namespace Sportradar.MTS.SDK.API.Internal.Senders
{
    public class TicketSender : TicketSenderBase
    {
        private readonly ITicketMapper<ITicket, TicketDTO> _ticketMapper;

        internal TicketSender(ITicketMapper<ITicket, TicketDTO> ticketMapper,
                              IRabbitMqPublisherChannel publisherChannel,
                              ConcurrentDictionary<string, TicketCacheItem> ticketCache,
                              IMtsChannelSettings mtsChannelSettings,
                              int ticketCacheTimeoutInSec)
            : base(publisherChannel, ticketCache, mtsChannelSettings, ticketCacheTimeoutInSec)
        {
            Contract.Requires(ticketMapper != null);

            _ticketMapper = ticketMapper;
        }

        /// <summary>
        /// Defines invariant members of the class
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_ticketMapper != null);
        }

        protected override string GetMappedDtoJsonMsg(ISdkTicket sdkTicket)
        {
            var ticket = sdkTicket as ITicket;
            var dto = _ticketMapper.Map(ticket);
            return dto.ToJson();
        }
    }
}
