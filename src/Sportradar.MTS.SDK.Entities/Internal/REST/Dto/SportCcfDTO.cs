/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Interfaces;
using Sportradar.MTS.SDK.Entities.Internal.REST.ClientApi;

namespace Sportradar.MTS.SDK.Entities.Internal.REST.Dto
{
    /// <summary>
    /// A data-transfer-object for customer confidence factor per sport
    /// </summary>
    public class SportCcfDTO : ISportCcf
    {
        public string SportId { get; }

        public long PrematchCcf { get; }

        public long LiveCcf { get; }


        internal SportCcfDTO(SportCcf sportCcf)
        {
            Contract.Requires(sportCcf != null);
            Contract.Requires(sportCcf.SportId != null);
            Contract.Requires(sportCcf.PrematchCcf != null);
            Contract.Requires(sportCcf.PrematchCcf.HasValue);
            Contract.Requires(sportCcf.LiveCcf != null);
            Contract.Requires(sportCcf.LiveCcf.HasValue);

            SportId = sportCcf.SportId;
            PrematchCcf = sportCcf.PrematchCcf.Value;
            LiveCcf = sportCcf.LiveCcf.Value;
        }
    }
}