/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using Dawn;

namespace Sportradar.MTS.SDK.Entities.Internal.REST.ClientApiImpl
{
    /// <summary>
    /// A data-transfer-object for Keycloak authorization
    /// </summary>
    internal class KeycloakAuthorization
    {
        public string AccessToken { get; }

        public DateTimeOffset Expires { get; }

        internal KeycloakAuthorization(Internal.Dto.ClientApi.AccessTokenDTO authorization)
        {
            Guard.Argument(authorization, nameof(authorization)).NotNull();
            Guard.Argument(authorization.Access_token, nameof(Internal.Dto.ClientApi.AccessTokenDTO.Access_token)).NotNull();
            Guard.Argument(authorization.Expires_in, nameof(Internal.Dto.ClientApi.AccessTokenDTO.Expires_in)).Positive();

            AccessToken = authorization.Access_token;
            Expires = DateTimeOffset.Now.AddSeconds(authorization.Expires_in);
        }
    }
}
