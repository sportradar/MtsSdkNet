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
        public const string IdPattern = "^[0-9A-Za-z:_-]*";

        public const string Version = "2.0";

        public const string Version21 = "2.1";

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

        [Pure]
        public static bool ValidStringId(string input, bool checkIdPattern, int minLength = -1, int maxLength = -1)
        {
            Contract.Requires(!string.IsNullOrEmpty(input));

            var valid = true;
            if (checkIdPattern)
            {
                valid = Regex.IsMatch(input, IdPattern);
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
    }
}