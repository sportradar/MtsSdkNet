/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Diagnostics.Contracts;
using System.Text;
using System.Text.RegularExpressions;
using Sportradar.MTS.SDK.Entities.Interfaces;
// ReSharper disable UnusedMember.Global

namespace Sportradar.MTS.SDK.Entities.Internal
{
    public static class TicketHelper
    {
        public const string BetIdPattern = "^[0-9A-Za-z:_-]*";

        public const string USerIdPattern = "^[0-9A-Za-z_-]*";

        public const string MtsTicketVersion = "2.2";

        public const int MaxPercent = 1000000;

        public static SdkTicketType GetTicketTypeFromTicket(ISdkTicket ticket)
        {
            if (ticket is ITicket)
            {
                return SdkTicketType.Ticket;
            }
            else if (ticket is ITicketCancel)
            {
                return  SdkTicketType.TicketCancel;
            }
            else if (ticket is ITicketAck)
            {
                return SdkTicketType.TicketAck;
            }
            else if (ticket is ITicketCancelAck)
            {
                return SdkTicketType.TicketCancelAck;
            }
            else if (ticket is ITicketResponse)
            {
                return SdkTicketType.TicketResponse;
            }
            else if (ticket is ITicketCancelResponse)
            {
                return SdkTicketType.TicketCancelResponse;
            }
            else if (ticket is ITicketReofferCancel)
            {
                return SdkTicketType.TicketReofferCancel;
            }
            else if (ticket is ITicketCashout)
            {
                return SdkTicketType.TicketCashout;
            }
            else if (ticket is ITicketCashoutResponse)
            {
                return SdkTicketType.TicketCashoutResponse;
            }
            throw new ArgumentOutOfRangeException();
        }

        public static SdkTicketType GetTicketAckTypeFromTicket(ISdkTicket ticket)
        {
            var ticketType = GetTicketTypeFromTicket(ticket);
            return ticketType == SdkTicketType.Ticket ? SdkTicketType.TicketAck : SdkTicketType.TicketCancelAck;
        }

        public static string GenerateTicketCorrelationId()
        {
            return $"n{Guid.NewGuid()}";
        }

        /// <summary>
        /// DateTime in UNIX time milliseconds
        /// </summary>
        public static long DateTimeToUnixTime(DateTime date)
        {
            var unixTime = date.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return (long)unixTime.TotalMilliseconds;
        }

        /// <summary>
        /// Convert Unix time to DateTime
        /// </summary>
        /// <param name="unixTime">The unix time.</param>
        /// <returns>DateTime.</returns>
        public static DateTime UnixTimeToDateTime(long unixTime)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddMilliseconds(unixTime).ToLocalTime();
            return dtDateTime;
        }

        public static ISdkTicket GetTicketInSpecificType(TicketCacheItem ci)
        {
            switch (ci.TicketType)
            {
                case SdkTicketType.Ticket:
                    return (ITicket) ci.Custom;
                case SdkTicketType.TicketAck:
                    return (ITicketAck)ci.Custom;
                case SdkTicketType.TicketCancel:
                    return (ITicketCancel)ci.Custom;
                case SdkTicketType.TicketCancelAck:
                    return (ITicketCancelAck)ci.Custom;
                case SdkTicketType.TicketResponse:
                    return (ITicketResponse)ci.Custom;
                case SdkTicketType.TicketCancelResponse:
                    return (ITicketCancelResponse)ci.Custom;
            }
            throw new ArgumentOutOfRangeException($"Unknown ticket type {ci.TicketType}.");
        }

        /// <summary>
        /// Try to parse the unparsable MSG
        /// </summary>
        /// <param name="rawData">The raw data.</param>
        /// <returns>System.String.</returns>
        public static string ParseUnparsableMsg(byte[] rawData)
        {
            try
            {
                return Encoding.UTF8.GetString(rawData);
            }
            catch (Exception)
            {
                // ignored
            }
            return string.Empty;
        }

        public static bool ValidateStringId(string input, bool checkIdPattern, bool useBetIdPattern = true, int minLength = -1, int maxLength = -1)
        {
            Contract.Requires(!string.IsNullOrEmpty(input));

            var valid = true;
            if (checkIdPattern)
            {
                valid = Regex.IsMatch(input, useBetIdPattern ? BetIdPattern : USerIdPattern);
            }
            if (valid && minLength >= 0)
            {
                valid = input.Length >= minLength;
            }
            if (valid && maxLength >= 0)
            {
                valid = input.Length <= maxLength;
            }
            return valid;
        }

        public static bool ValidateBetId(string input)
        {
            Contract.Requires(!string.IsNullOrEmpty(input));

            return ValidateStringId(input, true, true, 1, 128);
        }

        public static bool ValidateUserId(string input, int maxLength = 36)
        {
            Contract.Requires(!string.IsNullOrEmpty(input));

            return ValidateStringId(input, true, false, 1, maxLength);
        }

        public static bool ValidatePercent(int? percent)
        {
            return percent == null || percent > 0 && percent <= 1000000;
        }
    }
}