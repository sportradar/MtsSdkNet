/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Text;
using Newtonsoft.Json;

namespace Sportradar.MTS.SDK.Entities.Internal.REST.ClientApi
{
    /// <summary>
    /// </summary>
    public partial class MaxStakeResponse
    {
        /// <summary>
        ///     Ticket ID (from the original response)
        /// </summary>
        /// <value>Ticket ID (from the original response)</value>
        [JsonProperty(PropertyName = "ticketId")]
        public string TicketId { get; set; }

        /// <summary>
        ///     Maximum reoffer stake (quantity multiplied by 10000 and rounded to a long value)
        /// </summary>
        /// <value>Maximum reoffer stake (quantity multiplied by 10000 and rounded to a long value)</value>
        [JsonProperty(PropertyName = "maxStake")]
        public decimal? MaxStake { get; set; }

        /// <summary>
        ///     Gets or Sets TimestampUtc
        /// </summary>
        [JsonProperty(PropertyName = "timestampUtc")]
        public long? TimestampUtc { get; set; }


        /// <summary>
        ///     Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class MaxStakeResponse {\n");
            sb.Append("  TicketId: ").Append(TicketId).Append("\n");
            sb.Append("  MaxStake: ").Append(MaxStake).Append("\n");
            sb.Append("  TimestampUtc: ").Append(TimestampUtc).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}