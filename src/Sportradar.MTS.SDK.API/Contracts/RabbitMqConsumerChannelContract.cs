/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using RabbitMQ.Client.Events;
using Sportradar.MTS.SDK.API.Internal.RabbitMq;

namespace Sportradar.MTS.SDK.API.Contracts
{
    [ContractClassFor(typeof(IRabbitMqConsumerChannel))]
    internal abstract class RabbitMqConsumerChannelContract : IRabbitMqConsumerChannel
    {
        public abstract bool IsOpened { get; }

        public abstract void Close();

        public abstract void Open();

        [Pure]
        public event EventHandler<BasicDeliverEventArgs> ChannelMessageReceived;

        public void Open(IEnumerable<string> routingKeys)
        {
            Contract.Requires(routingKeys != null);
            Contract.Requires(routingKeys.Any());
            Contract.Ensures(IsOpened);
        }

        public void Open(string queueName, IEnumerable<string> routingKeys)
        {
            Contract.Requires(!string.IsNullOrEmpty(queueName));
            Contract.Requires(routingKeys != null);
            Contract.Requires(routingKeys.Any());
            Contract.Ensures(IsOpened);
        }
    }
}
