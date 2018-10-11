/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using RabbitMQ.Client.Events;
using Sportradar.MTS.SDK.API.Contracts;
using Sportradar.MTS.SDK.Common;

namespace Sportradar.MTS.SDK.API.Internal.RabbitMq
{
    /// <summary>
    /// Represents a contract implemented by classes used to connect to rabbit mq broker
    /// </summary>
    [ContractClass(typeof(RabbitMqConsumerChannelContract))]
    public interface IRabbitMqConsumerChannel : IOpenable
    {
        /// <summary>
        /// Opens the current channel and binds the created queue to provided routing keys
        /// </summary>
        /// <param name="routingKeys">A <see cref="IEnumerable{String}"/> specifying the routing keys of the constructed queue</param>
        void Open(IEnumerable<string> routingKeys);

        /// <summary>
        /// Opens the current channel and binds the created queue to provided routing keys
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="routingKeys">A <see cref="IEnumerable{String}"/> specifying the routing keys of the constructed queue</param>
        void Open(string queueName, IEnumerable<string> routingKeys);

        /// <summary>
        /// Occurs when the current channel received the data
        /// </summary>
        event EventHandler<BasicDeliverEventArgs> ChannelMessageReceived;
    }
}
