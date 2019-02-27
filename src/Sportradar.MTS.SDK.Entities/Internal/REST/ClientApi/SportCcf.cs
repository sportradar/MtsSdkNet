/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Text;
using Newtonsoft.Json;

namespace Sportradar.MTS.SDK.Entities.Internal.REST.ClientApi
{
    /// <summary>
    /// </summary>
    public partial class SportCcf
    {
        /// <summary>
        ///     Sport ID
        /// </summary>
        /// <value>Sport ID</value>
        [JsonProperty(PropertyName = "sportId")]
        public string SportId { get; set; }

        /// <summary>
        ///     Customer Confidence Factor for the sport for prematch selections (factor multiplied by 10000)
        /// </summary>
        /// <value>Customer Confidence Factor for the sport for prematch selections (factor multiplied by 10000)</value>
        [JsonProperty(PropertyName = "prematchCcf")]
        public long? PrematchCcf { get; set; }

        /// <summary>
        ///     Customer Confidence Factor for the sport for live selections (factor multiplied by 10000)
        /// </summary>
        /// <value>Customer Confidence Factor for the sport for live selections (factor multiplied by 10000)</value>
        [JsonProperty(PropertyName = "liveCcf")]
        public long? LiveCcf { get; set; }


        /// <summary>
        ///     Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class SportCcf {\n");
            sb.Append("  SportId: ").Append(SportId).Append("\n");
            sb.Append("  PrematchCcf: ").Append(PrematchCcf).Append("\n");
            sb.Append("  LiveCcf: ").Append(LiveCcf).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}