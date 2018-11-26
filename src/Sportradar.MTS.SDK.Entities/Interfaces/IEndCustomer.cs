/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Contracts;

namespace Sportradar.MTS.SDK.Entities.Interfaces
{
    /// <summary>
    /// Defines a contract for the identification of the end user (customer)
    /// </summary>
    [ContractClass(typeof(EndCustomerContract))]
    public interface IEndCustomer
    {
        /// <summary>
        /// Gets the end user's ip
        /// </summary>
        string Ip { get; }

        /// <summary>
        /// Gets the 2-letter ISO 639-1 language code
        /// </summary>
        string LanguageId { get; }

        /// <summary>
        /// Gets the device end user's device id
        /// </summary>
        string DeviceId { get; }

        /// <summary>
        /// Gets the end user's unique id (in client's system)
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the suggested CCF of the customer multiplied by 10000 and rounded to a long value
        /// </summary>
        long Confidence { get; }
    }
}