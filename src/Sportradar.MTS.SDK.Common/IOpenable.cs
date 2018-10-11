/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Common.Contracts;

namespace Sportradar.MTS.SDK.Common
{
    /// <summary>
    /// Defines a contract implemented by classes which can be opened and closed
    /// </summary>
    [ContractClass(typeof(OpenableContract))]
    public interface IOpenable
    {
        /// <summary>
        /// Gets a value indicating whether the current instance is opened
        /// </summary>
        bool IsOpened { get; }

        /// <summary>
        /// Opens the current instance.
        /// </summary>
        void Open();

        /// <summary>
        /// Closes the current instance
        /// </summary>
        void Close();
    }
}
