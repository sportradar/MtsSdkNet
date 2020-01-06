﻿/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections.Generic;
using Dawn;

namespace Sportradar.MTS.SDK.Entities.EventArguments
{
    /// <summary>
    /// Event arguments for the MqMessageDeserializationFailed event
    /// </summary>
    public class MessageDeserializationFailedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the <see cref="IEnumerable{Byte}"/> containing message unprocessed data
        /// </summary>
        public IEnumerable<byte> RawData { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageDeserializationFailedEventArgs"/> class
        /// </summary>
        /// <param name="rawData">the name of the message which could not be deserialized, or a null reference if message name could not be retrieved</param>
        public MessageDeserializationFailedEventArgs(IEnumerable<byte> rawData)
        {
            Guard.Argument(rawData, nameof(rawData)).NotNull().NotEmpty();

            RawData = rawData;
        }
    }
}