/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using Sportradar.MTS.SDK.Entities.Internal.REST.ClientApi;
using Sportradar.MTS.SDK.Entities.Internal.REST.Dto;

namespace Sportradar.MTS.SDK.Entities.Internal.REST
{
    /// <summary>
    /// Class CcfMapperFactory
    /// </summary>
    public class CcfMapperFactory : ISingleTypeMapperFactory<CcfResponse, CcfDTO>
    {
        /// <summary>
        /// Creates and returns an instance of Mapper for mapping <see cref="CcfResponse"/>
        /// </summary>
        /// <param name="data">A input instance which the created <see cref="CcfMapper"/> will map</param>
        /// <returns>New <see cref="CcfMapper" /> instance</returns>
        public ISingleTypeMapper<CcfDTO> CreateMapper(CcfResponse data)
        {
            return new CcfMapper(data);
        }
    }
}