/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Enums;
using Sportradar.MTS.SDK.Entities.Internal;

namespace Sportradar.MTS.SDK.Entities.Contracts
{
    [ContractClassFor(typeof(ISdkConfigurationInternal))]
    internal abstract class SdkConfigurationInternalContract : ISdkConfigurationInternal
    {
        public abstract string Username { get; }

        public abstract string Password { get; }

        public abstract string Host { get; }

        public abstract string VirtualHost { get; }

        public abstract bool UseSsl { get; }
        public abstract int NodeId { get; }

        [Pure]
        public int Port
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() > 0);
                return Contract.Result<int>();
            }
        }

        public abstract string ApiHost { get; }

        public abstract int BookmakerId { get; }

        public abstract int LimitId {get;}

        public abstract string Currency { get; }

        public abstract SenderChannel? Channel { get; }

        public abstract bool StatisticsEnabled { get; }

        public abstract int StatisticsTimeout { get; }

        public abstract int StatisticsRecordLimit { get; }

        public bool ExclusiveConsumer { get; }

        public abstract string AccessToken { get; }

        public abstract bool ProvideAdditionalMarketSpecifiers { get; }
    }
}
