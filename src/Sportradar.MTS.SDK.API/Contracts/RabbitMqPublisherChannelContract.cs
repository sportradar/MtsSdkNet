/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Sportradar.MTS.SDK.API.Internal.RabbitMq;
using Sportradar.MTS.SDK.Entities.EventArguments;

namespace Sportradar.MTS.SDK.API.Contracts
{
    [ContractClassFor(typeof(IRabbitMqPublisherChannel))]
    internal abstract class RabbitMqPublisherChannelContract : IRabbitMqPublisherChannel
    {
        public abstract int UniqueId { get; }

        public event EventHandler<MessagePublishFailedEventArgs> MqMessagePublishFailed;

        public abstract bool IsOpened { get; }

        public abstract void Close();

        public abstract void Open();

        public void Open(IEnumerable<string> routingKeys)
        {
            Contract.Requires(routingKeys != null);
            Contract.Requires(routingKeys.Any());
            Contract.Ensures(IsOpened);
        }

        public IMqPublishResult Publish(byte[] msg, string routingKey, string correlationId, string replyRoutingKey)
        {
            Contract.Requires(msg != null);
            Contract.Requires(!string.IsNullOrEmpty(routingKey));
            Contract.Ensures(Contract.Result<IMqPublishResult>() != null);
            return Contract.Result<IMqPublishResult>();
        }

        public Task<IMqPublishResult> PublishAsync(byte[] msg, string routingKey, string correlationId, string replyRoutingKey)
        {
            Contract.Requires(msg != null);
            Contract.Requires(!string.IsNullOrEmpty(routingKey));
            Contract.Ensures(Contract.Result<IMqPublishResult>() != null);
            return Contract.Result<Task<IMqPublishResult>>();
        }
    }
}
