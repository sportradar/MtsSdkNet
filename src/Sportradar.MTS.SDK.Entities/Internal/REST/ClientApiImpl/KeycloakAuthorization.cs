/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */

using System;
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Internal.Dto.ClientApi;

namespace Sportradar.MTS.SDK.Entities.Internal.REST.ClientApiImpl
{
    /// <summary>
    /// A data-transfer-object for Keycloak authorization
    /// </summary>
    public class KeycloakAuthorization
    {
        public string AccessToken { get; }

        public DateTimeOffset Expires { get; }

        internal KeycloakAuthorization(AccessTokenDTO authorization)
        {
            Contract.Requires(authorization != null);
            Contract.Requires(authorization.Access_token != null);
            Contract.Requires(authorization.Expires_in > 0);

            AccessToken = authorization.Access_token;
            Expires = DateTimeOffset.Now.AddSeconds(authorization.Expires_in);
        }
    }
}
