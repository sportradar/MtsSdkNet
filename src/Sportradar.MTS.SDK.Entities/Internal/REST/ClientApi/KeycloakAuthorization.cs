/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using Newtonsoft.Json;

namespace Sportradar.MTS.SDK.Entities.Internal.REST.ClientApi
{
    public partial class KeycloakAuthorization
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty("refresh_expires_in")]
        public long RefreshExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("not-before-policy")]
        public long NotBeforePolicy { get; set; }

        [JsonProperty("session_state")]
        public Guid SessionState { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }
    }
}
