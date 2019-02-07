/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Sportradar.MTS.SDK.Entities.Interfaces;
using Sportradar.MTS.SDK.Entities.Internal.REST.ClientApi;

namespace Sportradar.MTS.SDK.Entities.Internal.REST.Dto
{
    /// <summary>
    /// A data-transfer-object for customer confidence factor
    /// </summary>
    public partial class CcfDTO : ICcf
    {
        public long Ccf { get; }

        public IEnumerable<ISportCcf> SportCcfDetails { get; }

        internal CcfDTO(CcfResponse ccfResponse)
        {
            Contract.Requires(ccfResponse != null);
            Contract.Requires(ccfResponse.Ccf != null);
            Contract.Requires(ccfResponse.Ccf.HasValue);

            Ccf = ccfResponse.Ccf.Value;
            var sportCcfDetails = ccfResponse.SportCcfDetails ?? new List<SportCcf>();
            SportCcfDetails = sportCcfDetails.Select(d => new SportCcfDTO(d)).ToList();
        }
    }
}
