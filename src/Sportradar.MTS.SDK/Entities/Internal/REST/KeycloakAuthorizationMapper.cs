/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using Dawn;
using Sportradar.MTS.SDK.Entities.Internal.REST.ClientApiImpl;

namespace Sportradar.MTS.SDK.Entities.Internal.REST
{
    internal class KeycloakAuthorizationMapper : ISingleTypeMapper<KeycloakAuthorization>
    {
        /// <summary>
        /// A <see cref="Internal.Dto.ClientApi.AccessTokenDTO"/> instance containing data used to construct <see cref="KeycloakAuthorization"/> instance
        /// </summary>
        private readonly Internal.Dto.ClientApi.AccessTokenDTO _data;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeycloakAuthorizationMapper"/> class
        /// </summary>
        /// <param name="data">A <see cref="AccessTokenDTO"/> instance containing data used to construct <see cref="KeycloakAuthorization"/> instance</param>
        internal KeycloakAuthorizationMapper(Internal.Dto.ClientApi.AccessTokenDTO data)
        {
            Guard.Argument(data, nameof(data)).NotNull();

            _data = data;
        }

        /// <summary>
        /// Maps it's data to <see cref="KeycloakAuthorization"/> instance
        /// </summary>
        /// <returns>The created <see cref="KeycloakAuthorization"/> instance</returns>
        KeycloakAuthorization ISingleTypeMapper<KeycloakAuthorization>.Map()
        {
            return new KeycloakAuthorization(_data);
        }
    }
}