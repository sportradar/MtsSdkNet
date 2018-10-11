/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

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
        /// <param name="rawData">the name of the message which could not be deserialized, or a null reference if message name could
        ///                         not be retrieved</param>
        public MessageDeserializationFailedEventArgs(IEnumerable<byte> rawData)
        {
            Contract.Requires(rawData != null);
            Contract.Requires(rawData.Any());

            RawData = rawData;
        }
    }
}