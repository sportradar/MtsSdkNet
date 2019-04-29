/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using Sportradar.MTS.SDK.Entities.Internal.REST.ClientApi;
using Sportradar.MTS.SDK.Entities.Internal.REST.Dto;

namespace Sportradar.MTS.SDK.Entities.Internal.REST
{
    /// <summary>
    /// Class KeycloakAuthorizationMapperFactory
    /// </summary>
    public class KeycloakAuthorizationMapperFactory : ISingleTypeMapperFactory<KeycloakAuthorization, KeycloakAuthorizationDTO>
    {
        /// <summary>
        /// Creates and returns an instance of Mapper for mapping <see cref="KeycloakAuthorization"/>
        /// </summary>
        /// <param name="data">A input instance which the created <see cref="KeycloakAuthorizationMapper"/> will map</param>
        /// <returns>New <see cref="KeycloakAuthorizationMapper" /> instance</returns>
        public ISingleTypeMapper<KeycloakAuthorizationDTO> CreateMapper(KeycloakAuthorization data)
        {
            return new KeycloakAuthorizationMapper(data);
        }
    }
}