﻿/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Common.Internal;
using Sportradar.MTS.SDK.Entities.Internal;

namespace Sportradar.MTS.SDK.API.Internal.RabbitMq
{
    public class RabbitServer : IRabbitServer
    {
        public string Username { get; }
        public string Password { get; }
        public bool UseSsl { get; }
        public string VirtualHost { get; }
        public int Port { get; }
        public string HostAddress { get; }
        public IDictionary<string, object> ClientProperties { get; }
        public bool AutomaticRecovery { get; }
        public ushort HeartBeat { get; }

        public RabbitServer(ISdkConfigurationInternal config)
        {
            Contract.Requires(config != null);

            Username = config.Username;
            Password = config.Password;
            UseSsl = config.UseSsl;
            VirtualHost = config.VirtualHost;
            Port = config.Port;
            HostAddress = config.Host;

            ClientProperties = new Dictionary<string, object>
            {
                {"SrMtsSdkType", ".net"},
                {"SrMtsSdkVersion", SdkInfo.GetVersion()},
                {"SrMtsSdkInit", $"{DateTime.Now:yyyyMMddHHmm}"},
                {"connection_name", "RabbitMQ / Net"}
            };

            AutomaticRecovery = true;

            HeartBeat = 45;
        }
    }
}
