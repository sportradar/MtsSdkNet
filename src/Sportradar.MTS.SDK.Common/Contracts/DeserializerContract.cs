/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using System.IO;
using Sportradar.MTS.SDK.Common.Internal;

namespace Sportradar.MTS.SDK.Common.Contracts
{
    [ContractClassFor(typeof(IDeserializer<>))]
    internal abstract class DeserializerContract<T> : IDeserializer<T> where T : class
    {
        public T Deserialize(Stream stream)
        {
            Contract.Requires(stream != null);
            Contract.Ensures(Contract.Result<T>() != null);
            return Contract.Result<T>();
        }

        public T1 Deserialize<T1>(Stream stream) where T1 : T
        {
            Contract.Requires(stream != null);
            Contract.Ensures(Contract.Result<T1>() != null);
            return Contract.Result<T1>();
        }

        public T1 Deserialize<T1>(string input) where T1 : T
        {
            Contract.Requires(!string.IsNullOrEmpty(input));
            Contract.Ensures(Contract.Result<T1>() != null);
            return Contract.Result<T1>();
        }
    }
}
