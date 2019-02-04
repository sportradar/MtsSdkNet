/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Sportradar.MTS.SDK.Entities.Internal.REST.ClientApi
{
    /// <summary>
    /// </summary>
    public partial class CcfResponse
    {
        /// <summary>
        ///     Customer Confidence Factor (factor multiplied by 10000)
        /// </summary>
        /// <value>Customer Confidence Factor (factor multiplied by 10000)</value>
        [JsonProperty(PropertyName = "ccf")]
        public long? Ccf { get; set; }

        /// <summary>
        ///     CCF values for sport and prematch/live (if set for customer)
        /// </summary>
        /// <value>CCF values for sport and prematch/live (if set for customer)</value>
        [JsonProperty(PropertyName = "sportCcfDetails")]
        public List<SportCcf> SportCcfDetails { get; set; }


        /// <summary>
        ///     Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class CcfResponse {\n");
            sb.Append("  Ccf: ").Append(Ccf).Append("\n");
            sb.Append("  SportCcfDetails: ").Append(SportCcfDetails).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}