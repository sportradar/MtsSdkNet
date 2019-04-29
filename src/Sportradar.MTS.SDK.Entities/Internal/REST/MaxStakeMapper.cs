/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Internal.REST.ClientApi;
using Sportradar.MTS.SDK.Entities.Internal.REST.Dto;

namespace Sportradar.MTS.SDK.Entities.Internal.REST
{
    internal class MaxStakeMapper : ISingleTypeMapper<MaxStakeDTO>
    {
        /// <summary>
        /// A <see cref="MaxStakeResponse"/> instance containing data used to construct <see cref="MaxStakeDTO"/> instance
        /// </summary>
        private readonly MaxStakeResponse _data;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaxStakeMapper"/> class
        /// </summary>
        /// <param name="data">A <see cref="MaxStakeResponse"/> instance containing data used to construct <see cref="MaxStakeDTO"/> instance</param>
        internal MaxStakeMapper(MaxStakeResponse data)
        {
            Contract.Requires(data != null);

            _data = data;
        }

        /// <summary>
        /// Maps it's data to <see cref="MaxStakeDTO"/> instance
        /// </summary>
        /// <returns>The created <see cref="MaxStakeDTO"/> instance</returns>
        MaxStakeDTO ISingleTypeMapper<MaxStakeDTO>.Map()
        {
            return new MaxStakeDTO(_data);
        }
    }
}