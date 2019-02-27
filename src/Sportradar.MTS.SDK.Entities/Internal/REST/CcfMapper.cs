/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */

using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Internal.REST.ClientApi;
using Sportradar.MTS.SDK.Entities.Internal.REST.Dto;

namespace Sportradar.MTS.SDK.Entities.Internal.REST
{
    internal class CcfMapper : ISingleTypeMapper<CcfDTO>
    {
        /// <summary>
        /// A <see cref="CcfResponse"/> instance containing data used to construct <see cref="CcfDTO"/> instance
        /// </summary>
        private readonly CcfResponse _data;

        /// <summary>
        /// Initializes a new instance of the <see cref="CcfMapper"/> class
        /// </summary>
        /// <param name="data">A <see cref="CcfResponse"/> instance containing data used to construct <see cref="CcfDTO"/> instance</param>
        internal CcfMapper(CcfResponse data)
        {
            Contract.Requires(data != null);

            _data = data;
        }

        /// <summary>
        /// Maps it's data to <see cref="CcfDTO"/> instance
        /// </summary>
        /// <returns>The created <see cref="CcfDTO"/> instance</returns>
        CcfDTO ISingleTypeMapper<CcfDTO>.Map()
        {
            return new CcfDTO(_data);
        }
    }
}