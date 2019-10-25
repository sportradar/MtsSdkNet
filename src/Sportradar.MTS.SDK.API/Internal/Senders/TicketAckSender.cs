/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.API.Internal.Mappers;
using Sportradar.MTS.SDK.API.Internal.RabbitMq;
using Sportradar.MTS.SDK.Entities.Interfaces;
using Sportradar.MTS.SDK.Entities.Internal;
using Sportradar.MTS.SDK.Entities.Internal.Dto.TicketAck;

namespace Sportradar.MTS.SDK.API.Internal.Senders
{
    public class TicketAckSender : TicketSenderBase
    {
        private readonly ITicketMapper<ITicketAck, TicketAckDTO> _ticketMapper;

        internal TicketAckSender(ITicketMapper<ITicketAck, TicketAckDTO> ticketMapper,
                              IRabbitMqPublisherChannel publisherChannel,
                              ConcurrentDictionary<string, TicketCacheItem> ticketCache,
                              IMtsChannelSettings mtsChannelSettings,
                              IRabbitMqChannelSettings rabbitMqChannelSettings)
            : base(publisherChannel, ticketCache, mtsChannelSettings, rabbitMqChannelSettings)
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
            var ticket = sdkTicket as ITicketAck;
            var dto = _ticketMapper.Map(ticket);
            Contract.Assume(dto != null);
            return dto.ToJson();
        }
    }
}
