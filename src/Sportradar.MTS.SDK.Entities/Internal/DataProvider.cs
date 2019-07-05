﻿/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Sportradar.MTS.SDK.Common.Exceptions;
using Sportradar.MTS.SDK.Common.Internal;
using Sportradar.MTS.SDK.Common.Internal.Rest;
using Sportradar.MTS.SDK.Entities.Internal.REST;

namespace Sportradar.MTS.SDK.Entities.Internal
{
    /// <summary>
    /// An implementation of the <see cref="IDataProvider{T}"/> which fetches the data, deserializes it and than maps / converts it
    /// to the output type
    /// </summary>
    /// <typeparam name="TIn">Specifies the type of DTO instance which will be mapped to returned instance</typeparam>
    /// <typeparam name="TOut">Specifies the type of instances provided</typeparam>
    /// <seealso cref="IDataProvider{T}" />
    public class DataProvider<TIn, TOut> : IDataProvider<TOut> where TIn : RestMessage where TOut : class
    {
        /// <summary>
        /// A <see cref="IDataFetcher"/> used to fetch the data
        /// </summary>
        private readonly IDataFetcher _fetcher;

        /// <summary>
        /// A <see cref="IDataPoster"/> used to fetch the data
        /// </summary>
        private readonly IDataPoster _poster;

        /// <summary>
        /// A <see cref="IDeserializer{T}"/> used to deserialize the fetch data
        /// </summary>
        private readonly IDeserializer<TIn> _deserializer;

        /// <summary>
        /// A <see cref="ISingleTypeMapperFactory{T, T1}"/> used to construct instances of <see cref="ISingleTypeMapper{T}"/>
        /// </summary>
        private readonly ISingleTypeMapperFactory<TIn, TOut> _mapperFactory;

        /// <summary>
        /// The url format specifying the url of the resources fetched by the fetcher
        /// </summary>
        private readonly string _uriFormat;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProvider{T, T1}" /> class
        /// </summary>
        /// <param name="uriFormat">The url format specifying the url of the resources fetched by the fetcher</param>
        /// <param name="fetcher">A <see cref="IDataFetcher" /> used to fetch the data</param>
        /// <param name="poster">A <see cref="IDataPoster" /> used to fetch the data</param>
        /// <param name="deserializer">A <see cref="IDeserializer{T}" /> used to deserialize the fetch data</param>
        /// <param name="mapperFactory">A <see cref="ISingleTypeMapperFactory{T, T1}" /> used to construct instances of <see cref="ISingleTypeMapper{T}" /></param>
        public DataProvider(string uriFormat, IDataFetcher fetcher, IDataPoster poster, IDeserializer<TIn> deserializer, ISingleTypeMapperFactory<TIn, TOut> mapperFactory)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(uriFormat));
            Contract.Requires(fetcher != null);
            Contract.Requires(poster != null);
            Contract.Requires(deserializer != null);
            Contract.Requires(mapperFactory != null);

            _uriFormat = uriFormat;
            _fetcher = fetcher;
            _poster = poster;
            _deserializer = deserializer;
            _mapperFactory = mapperFactory;
        }

        /// <summary>
        /// Defines object invariants used by the code contracts
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(!string.IsNullOrWhiteSpace(_uriFormat));
            Contract.Invariant(_fetcher != null);
            Contract.Invariant(_poster != null);
            Contract.Invariant(_deserializer != null);
            Contract.Invariant(_mapperFactory != null);
        }

        /// <summary>
        /// Fetches and deserializes the data from the provided <see cref="Uri"/>.
        /// </summary>
        /// <param name="authorization">The value of authorization header</param>
        /// <param name="uri">A <see cref="Uri"/> specifying the data location</param>
        /// <returns>A <see cref="Task{T}"/> representing the ongoing operation</returns>
        protected async Task<TOut> GetDataAsyncInternal(string authorization, Uri uri)
        {
            Contract.Requires(uri != null);

            var stream = await _fetcher.GetDataAsync(authorization, uri).ConfigureAwait(false);
            var deserializedObject = _deserializer.Deserialize(stream);
            return _mapperFactory.CreateMapper(deserializedObject).Map();
        }

        /// <summary>
        /// Fetches and deserializes the data from the provided <see cref="Uri"/>.
        /// </summary>
        /// <param name="content">A <see cref="HttpContent"/> to be posted to the specific <see cref="Uri"/></param>
        /// <param name="authorization">The value of authorization header</param>
        /// <param name="uri">A <see cref="Uri"/> specifying the data location</param>
        /// <returns>A <see cref="Task{TOut}"/> representing the ongoing operation</returns>
        protected async Task<TOut> PostDataAsyncInternal(string authorization, HttpContent content, Uri uri)
        {
            Contract.Requires(uri != null);

            var responseMessage = await _poster.PostDataAsync(authorization, uri, content).ConfigureAwait(false);
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new CommunicationException($"Response StatusCode={responseMessage.StatusCode} does not indicate success.", uri.ToString(), responseMessage.StatusCode, null);
            }
            var stream = await responseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);
            var deserializedObject = _deserializer.Deserialize(stream);
            return _mapperFactory.CreateMapper(deserializedObject).Map();
        }

        /// <summary>
        /// Constructs and returns an <see cref="Uri"/> instance used to retrieve resource with specified <code>id</code>
        /// </summary>
        /// <param name="identifiers">Identifiers uniquely identifying the data to fetch</param>
        /// <returns>an <see cref="Uri"/> instance used to retrieve resource with specified <code>identifiers</code></returns>
        protected virtual Uri GetRequestUri(params object[] identifiers)
        {
            Contract.Requires(identifiers != null && identifiers.Any());
            Contract.Ensures(Contract.Result<Uri>() != null);

            return new Uri(string.Format(_uriFormat, identifiers));
        }

        /// <summary>
        /// Get the data as an asynchronous operation
        /// </summary>
        /// <param name="languageCode">A two letter language code of the <see cref="T:System.Globalization.CultureInfo" /></param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> representing the async operation</returns>
        /// <exception cref="CommunicationException">Failed to execute http get</exception>
        /// <exception cref="DeserializationException">The deserialization failed</exception>
        /// <exception cref="MappingException">The deserialized entity could not be mapped to entity used by the SDK</exception>
        public async Task<TOut> GetDataAsync(string languageCode)
        {
            return await GetDataAsync((string) null, languageCode).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the data as an asynchronous operation
        /// </summary>
        /// <param name="authorization">The value of authorization header</param>
        /// <param name="languageCode">A two letter language code of the <see cref="T:System.Globalization.CultureInfo" /></param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> representing the async operation</returns>
        /// <exception cref="CommunicationException">Failed to execute http get</exception>
        /// <exception cref="DeserializationException">The deserialization failed</exception>
        /// <exception cref="MappingException">The deserialized entity could not be mapped to entity used by the SDK</exception>
        public async Task<TOut> GetDataAsync(string authorization, string languageCode)
        {
            var uri = GetRequestUri(languageCode);
            return await GetDataAsyncInternal(authorization, uri).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously gets a <typeparamref name="TOut"/> instance specified by the provided identifiersA two letter language code of the <see cref="CultureInfo"/>
        /// </summary>
        /// <param name="identifiers">A list of identifiers uniquely specifying the instance to fetch</param>
        /// <returns>A <see cref="Task{T}"/> representing the async operation</returns>
        /// <exception cref="CommunicationException">Failed to execute http get</exception>
        /// <exception cref="DeserializationException">The deserialization failed</exception>
        /// <exception cref="MappingException">The deserialized entity could not be mapped to entity used by the SDK</exception>
        public async Task<TOut> GetDataAsync(params string[] identifiers)
        {
            return await GetDataAsync((string) null, identifiers).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously gets a <typeparamref name="TOut"/> instance specified by the provided identifiersA two letter language code of the <see cref="CultureInfo"/>
        /// </summary>
        /// <param name="authorization">The value of authorization header</param>
        /// <param name="identifiers">A list of identifiers uniquely specifying the instance to fetch</param>
        /// <returns>A <see cref="Task{T}"/> representing the async operation</returns>
        /// <exception cref="CommunicationException">Failed to execute http get</exception>
        /// <exception cref="DeserializationException">The deserialization failed</exception>
        /// <exception cref="MappingException">The deserialized entity could not be mapped to entity used by the SDK</exception>
        public async Task<TOut> GetDataAsync(string authorization, params string[] identifiers)
        {
            // ReSharper disable once CoVariantArrayConversion
            var uri = GetRequestUri(identifiers);
            return await GetDataAsyncInternal(authorization, uri).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously gets a <see cref="Task{TOut}"/> instance in language specified by the provided <code>languageCode</code>
        /// </summary>
        /// <param name="content">A <see cref="HttpContent"/> to be posted to the specific <see cref="Uri"/></param>
        /// <param name="languageCode">A two letter language code of the <see cref="CultureInfo"/></param>
        /// <returns>A <see cref="Task{TOut}"/> representing the async operation</returns>
        /// <exception cref="CommunicationException">Failed to execute http get</exception>
        /// <exception cref="DeserializationException">The deserialization failed</exception>
        /// <exception cref="MappingException">The deserialized entity could not be mapped to entity used by the SDK</exception>
        public async Task<TOut> PostDataAsync(HttpContent content, string languageCode)
        {
            return await PostDataAsync(content, null, languageCode).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously gets a <see cref="Task{TOut}"/> instance in language specified by the provided <code>languageCode</code>
        /// </summary>
        /// <param name="content">A <see cref="HttpContent"/> to be posted to the specific <see cref="Uri"/></param>
        /// <param name="authorization">The value of authorization header</param>
        /// <param name="languageCode">A two letter language code of the <see cref="CultureInfo"/></param>
        /// <returns>A <see cref="Task{TOut}"/> representing the async operation</returns>
        /// <exception cref="CommunicationException">Failed to execute http get</exception>
        /// <exception cref="DeserializationException">The deserialization failed</exception>
        /// <exception cref="MappingException">The deserialized entity could not be mapped to entity used by the SDK</exception>
        public async Task<TOut> PostDataAsync(string authorization, HttpContent content, string languageCode)
        {
            var uri = GetRequestUri(languageCode);
            return await PostDataAsyncInternal(authorization, content, uri).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously gets a <see cref="Task{TOut}"/> instance specified by the provided identifiersA two letter language code of the <see cref="CultureInfo"/>
        /// </summary>
        /// <param name="content">A <see cref="HttpContent"/> to be posted to the specific <see cref="Uri"/></param>
        /// <param name="identifiers">A list of identifiers uniquely specifying the instance to fetch</param>
        /// <returns>A <see cref="Task{TOut}"/> representing the async operation</returns>
        /// <exception cref="CommunicationException">Failed to execute http get</exception>
        /// <exception cref="DeserializationException">The deserialization failed</exception>
        /// <exception cref="MappingException">The deserialized entity could not be mapped to entity used by the SDK</exception>
        public async Task<TOut> PostDataAsync(HttpContent content, params string[] identifiers)
        {
            return await PostDataAsync(null, content, identifiers).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously gets a <see cref="Task{TOut}"/> instance specified by the provided identifiersA two letter language code of the <see cref="CultureInfo"/>
        /// </summary>
        /// <param name="content">A <see cref="HttpContent"/> to be posted to the specific <see cref="Uri"/></param>
        /// <param name="authorization">The value of authorization header</param>
        /// <param name="identifiers">A list of identifiers uniquely specifying the instance to fetch</param>
        /// <returns>A <see cref="Task{TOut}"/> representing the async operation</returns>
        /// <exception cref="CommunicationException">Failed to execute http get</exception>
        /// <exception cref="DeserializationException">The deserialization failed</exception>
        /// <exception cref="MappingException">The deserialized entity could not be mapped to entity used by the SDK</exception>
        public async Task<TOut> PostDataAsync(string authorization, HttpContent content, params string[] identifiers)
        {
            // ReSharper disable once CoVariantArrayConversion
            var uri = GetRequestUri(identifiers);
            return await PostDataAsyncInternal(authorization, content, uri).ConfigureAwait(false);
        }
    }
}