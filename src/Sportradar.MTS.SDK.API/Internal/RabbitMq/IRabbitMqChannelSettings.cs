/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
namespace Sportradar.MTS.SDK.API.Internal.RabbitMq
{
    /// <summary>
    /// Defines a contract for channel settings
    /// </summary>
    public interface IRabbitMqChannelSettings
    {
        /// <summary>
        /// Gets a value indicating whether the queue should be deleted on close
        /// </summary>
        bool DeleteQueueOnClose { get; }

        /// <summary>
        /// Gets a value indicating whether created queue is durable
        /// </summary>
        bool QueueIsDurable { get; }

        /// <summary>
        /// Gets a value indicating whether user acknowledgement enabled on queue
        /// </summary>
        bool UserAcknowledgementEnabled { get; }

        /// <summary>
        /// Specifies minimum allowed value of the inactivity value
        /// </summary>
        int HeartBeat { get; }

        /// <summary>
        /// The user acknowledgment batch limit for received messages
        /// </summary>
        int UserAcknowledgementBatchLimit { get; }

        /// <summary>
        /// The user acknowledgement timeout in seconds for received messages
        /// </summary>
        int UserAcknowledgementTimeoutInSeconds { get; }

        /// <summary>
        /// Gets the delivery mode of the publishing channel (persistent or non-persistent)
        /// </summary>
        bool UsePersistentDeliveryMode { get; }

        /// <summary>
        /// Gets the publish queue limit (0 - unlimited)
        /// </summary>
        int PublishQueueLimit { get; }

        /// <summary>
        /// Gets the timeout for items in publish queue
        /// </summary>
        /// <value>Default 15 seconds</value>
        int PublishQueueTimeoutInSec { get; }

        /// <summary>
        /// Gets a value indicating whether the rabbit consumer channel should be exclusive
        /// </summary>
        bool ExclusiveConsumer { get; }
    }
}
