/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
namespace Sportradar.MTS.SDK.API.Internal.RabbitMq
{
    public class RabbitMqChannelSettings : IRabbitMqChannelSettings
    {
        public bool DeleteQueueOnClose { get; }
        public bool QueueIsDurable { get; }
        public bool UserAcknowledgmentEnabled { get; }
        public int HeartBeat { get; }
        public int UserAcknowledgmentBatchLimit { get; }
        public int UserAcknowledgmentTimeoutInSeconds { get; }
        public bool UsePersistentDeliveryMode { get; }
        public int PublishQueueLimit { get; }
        public int PublishQueueTimeoutInSec { get; }
        public bool ExclusiveConsumer { get; }

        public RabbitMqChannelSettings( bool queueDurable = false,
                                        bool exclusiveConsumer = true,
                                        bool enableUserAqs = false,
                                        bool deleteQueueOnClose = true,
                                        int heartBeat = 0,
                                        int ackBatchLimit = 1,
                                        int ackTimeout = 60,
                                        bool usePersistentDeliveryMode = false,
                                        int publishQueueLimit = 0,
                                        int publishQueueTimeoutInSec = 15)
        {
            DeleteQueueOnClose = deleteQueueOnClose;
            QueueIsDurable = queueDurable;
            UserAcknowledgmentEnabled = enableUserAqs;
            HeartBeat = heartBeat;
            UserAcknowledgmentBatchLimit = ackBatchLimit;
            UserAcknowledgmentTimeoutInSeconds = ackTimeout;
            UsePersistentDeliveryMode = usePersistentDeliveryMode;
            PublishQueueLimit = publishQueueLimit;
            PublishQueueTimeoutInSec = publishQueueTimeoutInSec;
            ExclusiveConsumer = exclusiveConsumer;
        }
    }
}
