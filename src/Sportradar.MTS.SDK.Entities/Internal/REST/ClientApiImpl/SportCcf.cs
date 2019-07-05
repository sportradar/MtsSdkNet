/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */

using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Interfaces;
using Sportradar.MTS.SDK.Entities.Internal.Dto.ClientApi;

namespace Sportradar.MTS.SDK.Entities.Internal.REST.ClientApiImpl
{
    /// <summary>
    /// A data-transfer-object for customer confidence factor per sport
    /// </summary>
    public class SportCcf : ISportCcf
    {
        public string SportId { get; }

        public long PrematchCcf { get; }

        public long LiveCcf { get; }

        internal SportCcf(Anonymous sportCcf)
        {
            Contract.Requires(sportCcf != null);
            Contract.Requires(sportCcf.SportId != null);

            SportId = sportCcf.SportId;
            PrematchCcf = sportCcf.PrematchCcf;
            LiveCcf = sportCcf.LiveCcf;
        }
    }
}