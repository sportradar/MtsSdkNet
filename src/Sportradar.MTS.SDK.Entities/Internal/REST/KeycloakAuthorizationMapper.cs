/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Internal.REST.ClientApi;
using Sportradar.MTS.SDK.Entities.Internal.REST.Dto;

namespace Sportradar.MTS.SDK.Entities.Internal.REST
{
    internal class KeycloakAuthorizationMapper : ISingleTypeMapper<KeycloakAuthorizationDTO>
    {
        /// <summary>
        /// A <see cref="KeycloakAuthorization"/> instance containing data used to construct <see cref="KeycloakAuthorizationDTO"/> instance
        /// </summary>
        private readonly KeycloakAuthorization _data;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeycloakAuthorizationMapper"/> class
        /// </summary>
        /// <param name="data">A <see cref="KeycloakAuthorization"/> instance containing data used to construct <see cref="KeycloakAuthorizationDTO"/> instance</param>
        internal KeycloakAuthorizationMapper(KeycloakAuthorization data)
        {
            Contract.Requires(data != null);

            _data = data;
        }

        /// <summary>
        /// Maps it's data to <see cref="KeycloakAuthorizationDTO"/> instance
        /// </summary>
        /// <returns>The created <see cref="KeycloakAuthorizationDTO"/> instance</returns>
        KeycloakAuthorizationDTO ISingleTypeMapper<KeycloakAuthorizationDTO>.Map()
        {
            return new KeycloakAuthorizationDTO(_data);
        }
    }
}