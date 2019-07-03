/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Internal.TicketImpl
{
    public class EndCustomer : IEndCustomer
    {
        public string Ip { get; }
        public string LanguageId { get; }
        public string DeviceId { get; }

        /// <summary>
        /// Gets the end user's unique id (in client's system)
        /// </summary>
        public string Id { get; }

        public long Confidence { get; }

        [JsonConstructor]
        public EndCustomer(string ip, string languageId, string deviceId, string id, long confidence)
        {
            Contract.Requires(string.IsNullOrEmpty(languageId) || languageId.Length == 2);
            Contract.Requires(string.IsNullOrEmpty(deviceId) || TicketHelper.ValidateUserId(deviceId));
            Contract.Requires(string.IsNullOrEmpty(id) || TicketHelper.ValidateUserId(id));
            Contract.Requires(confidence >= 0);

            if (!string.IsNullOrEmpty(ip))
            {
                IPAddress.Parse(ip);
                Ip = ip;
            }
            LanguageId = languageId;
            DeviceId = deviceId;
            Id = id;
            Confidence = confidence;
        }

        public EndCustomer(IPAddress ip, string languageId, string deviceId, string id, long confidence)
        {
            Contract.Requires(string.IsNullOrEmpty(languageId) || languageId.Length == 2);
            Contract.Requires(string.IsNullOrEmpty(deviceId) || TicketHelper.ValidateUserId(deviceId));
            Contract.Requires(string.IsNullOrEmpty(id) || TicketHelper.ValidateUserId(id));
            Contract.Requires(confidence >= 0);

            Ip = ip?.ToString();
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
            Contract.Invariant(string.IsNullOrEmpty(DeviceId) || TicketHelper.ValidateUserId(DeviceId));
            Contract.Invariant(string.IsNullOrEmpty(Id) || TicketHelper.ValidateUserId(Id));
            Contract.Invariant(Confidence >= 0);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue("Ip", Ip);
            info.AddValue("LanguageId", LanguageId);
            info.AddValue("DeviceId", DeviceId);
            info.AddValue("Id", Id);
            info.AddValue("Confidence", Confidence);
        }
    }
}