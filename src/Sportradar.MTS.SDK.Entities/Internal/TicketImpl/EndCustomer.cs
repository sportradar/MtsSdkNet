/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Runtime.Serialization;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Internal.TicketImpl
{
    public class EndCustomer : IEndCustomer, ISerializable
    {
        public IPAddress Ip { get; }
        public string LanguageId { get; }
        public string DeviceId { get; }

        /// <summary>
        /// Gets the end user's unique id (in client's system)
        /// </summary>
        public string Id { get; }

        public long Confidence { get; }

        public EndCustomer(string ip, string languageId, string deviceId, string id, long confidence)
        {
            Contract.Requires(string.IsNullOrEmpty(languageId) || languageId.Length == 2);
            Contract.Requires(string.IsNullOrEmpty(deviceId) || TicketHelper.ValidStringId(deviceId, true, 1, 36));
            Contract.Requires(string.IsNullOrEmpty(id) || TicketHelper.ValidStringId(id, true, 1, 36));
            Contract.Requires(confidence >= 0);

            if (!string.IsNullOrEmpty(ip))
            {
                Ip = IPAddress.Parse(ip);
            }
            LanguageId = languageId;
            DeviceId = deviceId;
            Id = id;
            Confidence = confidence;
        }

        public EndCustomer(IPAddress ip, string languageId, string deviceId, string id, long confidence)
        {
            Contract.Requires(string.IsNullOrEmpty(languageId) || languageId.Length == 2);
            Contract.Requires(string.IsNullOrEmpty(deviceId) || TicketHelper.ValidStringId(deviceId, true, 1, 36));
            Contract.Requires(string.IsNullOrEmpty(id) || TicketHelper.ValidStringId(id, true, 1, 36));
            Contract.Requires(confidence >= 0);

            Ip = ip;
            LanguageId = languageId;
            DeviceId = deviceId;
            Id = id;
            Confidence = confidence;
        }

        /// <summary>
        /// Defines invariant members of the class
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(string.IsNullOrEmpty(LanguageId) || LanguageId.Length == 2);
            Contract.Invariant(string.IsNullOrEmpty(DeviceId) || TicketHelper.ValidStringId(DeviceId, true, 1, 36));
            Contract.Invariant(string.IsNullOrEmpty(Id) || TicketHelper.ValidStringId(Id, true, 1, 36));
            Contract.Invariant(Confidence >= 0);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue("Ip", Ip?.ToString());
            info.AddValue("LanguageId", LanguageId);
            info.AddValue("DeviceId", DeviceId);
            info.AddValue("Id", Id);
            info.AddValue("Confidence", Confidence);
        }
    }
}