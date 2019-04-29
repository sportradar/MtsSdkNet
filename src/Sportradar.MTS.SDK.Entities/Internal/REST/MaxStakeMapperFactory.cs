/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using Sportradar.MTS.SDK.Entities.Internal.REST.ClientApi;
using Sportradar.MTS.SDK.Entities.Internal.REST.Dto;

namespace Sportradar.MTS.SDK.Entities.Internal.REST
{
    /// <summary>
    /// Class MarketDescriptionsMapperFactory
    /// </summary>
    public class MaxStakeMapperFactory : ISingleTypeMapperFactory<MaxStakeResponse, MaxStakeDTO>
    {
        /// <summary>
        /// Creates and returns an instance of Mapper for mapping <see cref="MaxStakeResponse"/>
        /// </summary>
        /// <param name="data">A input instance which the created <see cref="MaxStakeMapper"/> will map</param>
        /// <returns>New <see cref="MaxStakeMapper" /> instance</returns>
        public ISingleTypeMapper<MaxStakeDTO> CreateMapper(MaxStakeResponse data)
        {
            return new MaxStakeMapper(data);
        }
    }
}