/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Enums;

namespace Sportradar.MTS.SDK.Entities.Contracts
{
    [ContractClassFor(typeof(ISdkConfiguration))]
    internal abstract class SdkConfigurationContract : ISdkConfiguration
    {
        [Pure]
        public string Username {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return Contract.Result<string>();
            }
        }

        [Pure]
        public string Password
        {
            [Pure]
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return Contract.Result<string>();
            }
        }

        [Pure]
        public string Host
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return Contract.Result<string>();
            }
        }

        public int Port
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() > 0);
                return Contract.Result<int>();
            }
        }

        [Pure]
        public string VirtualHost
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return Contract.Result<string>();
            }
        }

        [Pure]
        public bool UseSsl => Contract.Result<bool>();

        [Pure]
        public int NodeId => Contract.Result<int>();

        [Pure]
        public int BookmakerId => Contract.Result<int>();

        [Pure]
        public int LimitId => Contract.Result<int>();

        [Pure]
        public string Currency => Contract.Result<string>();

        [Pure]
        public SenderChannel? Channel => Contract.Result<SenderChannel?>();

        [Pure]
        public bool StatisticsEnabled => Contract.Result<bool>();

        [Pure]
        public int StatisticsTimeout => Contract.Result<int>();

        [Pure]
        public int StatisticsRecordLimit => Contract.Result<int>();

        [Pure]
        public bool ExclusiveConsumer => Contract.Result<bool>();

        [Pure]
        public string KeycloakHost => Contract.Result<string>();

        [Pure]
        public string KeycloakUsername => Contract.Result<string>();

        [Pure]
        public string KeycloakPassword => Contract.Result<string>();

        [Pure]
        public string KeycloakSecret => Contract.Result<string>();

        [Pure]
        public string MtsClientApiHost => Contract.Result<string>();

        [Pure]
        public string AccessToken => Contract.Result<string>();

        [Pure]
        public bool ProvideAdditionalMarketSpecifiers => Contract.Result<bool>();
    }
}
