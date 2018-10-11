/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.Entities.Internal.Dto.Ticket
{
    public partial class EndCustomer
    {
        public EndCustomer()
        { }

        public EndCustomer(string id, string deviceId, string ip, string languageId, long confidence)
        {
            Id = string.IsNullOrEmpty(id) ? null : id;
            Confidence = confidence <= 0 ? (long?) null : confidence;
            DeviceId = string.IsNullOrEmpty(deviceId) ? null : deviceId;
            Ip = string.IsNullOrEmpty(ip) ? null : ip;
            LanguageId = string.IsNullOrEmpty(languageId) ? null : languageId;
        }

        public EndCustomer(IEndCustomer customer)
        {
            Contract.Requires(customer != null);

            Id = string.IsNullOrEmpty(customer.Id) ? null : customer.Id;
            Confidence = customer.Confidence > 0 ? customer.Confidence : (long?)null;
            DeviceId = string.IsNullOrEmpty(customer.DeviceId) ? null : customer.DeviceId;
            Ip = customer.Ip?.ToString();
            LanguageId = string.IsNullOrEmpty(customer.LanguageId) ? null : customer.LanguageId;
        }
    }
}