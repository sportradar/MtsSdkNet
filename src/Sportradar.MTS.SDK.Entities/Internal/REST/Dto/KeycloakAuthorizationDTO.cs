/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Internal.REST.ClientApi;

namespace Sportradar.MTS.SDK.Entities.Internal.REST.Dto
{
    /// <summary>
    /// A data-transfer-object for Keycloak authorization
    /// </summary>
    public class KeycloakAuthorizationDTO
    {
        public string AccessToken { get; }

        public DateTimeOffset Expires { get; }

        internal KeycloakAuthorizationDTO(KeycloakAuthorization authorization)
        {
            Contract.Requires(authorization != null);
            Contract.Requires(authorization.AccessToken != null);
            Contract.Requires(authorization.ExpiresIn > 0);

            AccessToken = authorization.AccessToken;
            Expires = DateTimeOffset.Now.AddSeconds(authorization.ExpiresIn);
        }
    }
}
