/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Globalization;
using System.Threading.Tasks;
using Sportradar.MTS.SDK.Common.Exceptions;

namespace Sportradar.MTS.SDK.Entities.Internal
{
    /// <summary>
    /// Defines a contract implemented by classes used to provide data specified by it's id
    /// </summary>
    /// <typeparam name="T">Specifies the type of data returned by this <see cref="IDataProvider{T}"/></typeparam>
    public interface IDataProvider<T> where T : class
    {
        /// <summary>
        /// Asynchronously gets a <see cref="Task{T}"/> instance in language specified by the provided <code>languageCode</code>
        /// </summary>
        /// <param name="languageCode">A two letter language code of the <see cref="CultureInfo"/></param>
        /// <returns>A <see cref="Task{T}"/> representing the async operation</returns>
        /// <exception cref="CommunicationException">Failed to execute http get</exception>
        /// <exception cref="DeserializationException">The deserialization failed</exception>
        /// <exception cref="MappingException">The deserialized entity could not be mapped to entity used by the SDK</exception>
        Task<T> GetDataAsync(string languageCode);

        /// <summary>
        /// Asynchronously gets a <see cref="Task{T}"/> instance specified by the provided identifiersA two letter language code of the <see cref="CultureInfo"/>
        /// </summary>
        /// <param name="identifiers">A list of identifiers uniquely specifying the instance to fetch</param>
        /// <returns>A <see cref="Task{T}"/> representing the async operation</returns>
        /// <exception cref="CommunicationException">Failed to execute http get</exception>
        /// <exception cref="DeserializationException">The deserialization failed</exception>
        /// <exception cref="MappingException">The deserialized entity could not be mapped to entity used by the SDK</exception>
        Task<T> GetDataAsync(params string[] identifiers);
    }
}