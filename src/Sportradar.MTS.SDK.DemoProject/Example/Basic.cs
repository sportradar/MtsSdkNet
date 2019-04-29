﻿/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using log4net;
using Sportradar.MTS.SDK.API;
using Sportradar.MTS.SDK.Entities.Builders;
using Sportradar.MTS.SDK.Entities.Enums;
using Sportradar.MTS.SDK.Entities.EventArguments;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.DemoProject.Example
{
    /// <summary>
    /// Basic example for creating and sending ticket
    /// </summary>
    public class Basic
    {
        /// <summary>
        /// The log
        /// </summary>
        private readonly ILog _log;

        /// <summary>
        /// The MTS SDK instance
        /// </summary>
        private IMtsSdk _mtsSdk;

        private IBuilderFactory _factory;

        public Basic(ILog log)
        {
            _log = log;
        }

        public void Run()
        {
            _log.Info("Running the MTS SDK Basic example");

            _log.Info("Retrieving configuration from application configuration file");
            var config = MtsSdk.GetConfiguration();

            _log.Info("Creating root MTS SDK instance");
            _mtsSdk = new MtsSdk(config);

            _log.Info("Attaching to events");
            AttachToFeedEvents(_mtsSdk);

            _log.Info("Opening the sdk instance (creating and opening connection to the AMPQ broker)");
            _mtsSdk.Open();
            _factory = _mtsSdk.BuilderFactory;

            // create ticket (in order to be accepted, correct values must be entered)
            // values below are just for demonstration purposes and will not be accepted
            var r = new Random();
            var ticket = _factory.CreateTicketBuilder()
                .SetTicketId("ticketId-" + r.Next())
                .SetSender(_factory.CreateSenderBuilder()
                    .SetCurrency("EUR")
                    .SetEndCustomer(_factory.CreateEndCustomerBuilder()
                        .SetId("customerClientId-" + r.Next())
                        .SetConfidence(1)
                        .SetIp(IPAddress.Loopback)
                        .SetLanguageId("en")
                        .SetDeviceId("UsersDevice-" + r.Next())
                        .Build())
                    .Build())
                .AddBet(_factory.CreateBetBuilder()
                        .SetBetId("betId-" + r.Next())
                        .SetBetBonus(1)
                        .SetStake(11200, StakeType.Total)
                        .AddSelectedSystem(1)
                        .AddSelection(_factory.CreateSelectionBuilder()
                            .SetEventId("1")
                            //.SetIdUof(3, $"sr:match:{r.Next()}", 12, "1", string.Empty, null) // only one of the following is required to set selectionId (if you use this method, config property 'accessToken' must be provided)
                            .SetIdLo(1, 1, "1:1", "1")
                            .SetIdLcoo(1, 1, "1:1", "1")
                            .SetId("lcoo:409/1/*/YES")
                            .SetOdds(11000)
                            .Build())
                        .Build())
                .BuildTicket();

            // send ticket to the MTS. Since this is a non-blocking way of sending, the response will come on the event TicketResponseReceived
            _log.Info("Send ticket to the MTS.");
            _log.Info(ticket.ToJson());
            _mtsSdk.SendTicket(ticket);

            _log.Info("Example successfully executed. Hit <enter> to quit");
            Console.WriteLine(string.Empty);
            Console.ReadLine();

            _log.Info("Detaching from events");
            DetachFromFeedEvents(_mtsSdk);

            _log.Info("Closing the connection and disposing the instance");
            _mtsSdk.Close();

            _log.Info("Example stopped");
        }

        /// <summary>
        /// Attaches to events raised by <see cref="IMtsSdk"/>
        /// </summary>
        /// <param name="mtsSdk">A <see cref="IMtsSdk"/> instance </param>
        private void AttachToFeedEvents(IMtsSdk mtsSdk)
        {
            Contract.Requires(mtsSdk != null);

            _log.Info("Attaching to events");
            mtsSdk.SendTicketFailed += OnSendTicketFailed;
            mtsSdk.TicketResponseReceived += OnTicketResponseReceived;
            mtsSdk.UnparsableTicketResponseReceived += OnUnparsableTicketResponseReceived;
        }

        /// <summary>
        /// Detaches from events defined by <see cref="IMtsSdk"/>
        /// </summary>
        /// <param name="mtsSdk">A <see cref="IMtsSdk"/> instance</param>
        private void DetachFromFeedEvents(IMtsSdk mtsSdk)
        {
            Contract.Requires(mtsSdk != null);

            _log.Info("Detaching from events");
            mtsSdk.SendTicketFailed -= OnSendTicketFailed;
            mtsSdk.TicketResponseReceived -= OnTicketResponseReceived;
            mtsSdk.UnparsableTicketResponseReceived -= OnUnparsableTicketResponseReceived;
        }

        private void OnTicketResponseReceived(object sender, TicketResponseReceivedEventArgs e)
        {
            _log.Info($"Received {e.Type}Response for ticket '{e.Response.TicketId}'.");

            if (e.Type == TicketResponseType.Ticket)
            {
                HandleTicketResponse((ITicketResponse)e.Response);
            }
            else if (e.Type == TicketResponseType.TicketCancel)
            {
                HandleTicketCancelResponse((ITicketCancelResponse)e.Response);
            }
        }

        private void OnUnparsableTicketResponseReceived(object sender, UnparsableMessageEventArgs e)
        {
            _log.Info($"Received unparsable ticket response: {e.Body}.");
        }

        private void OnSendTicketFailed(object sender, TicketSendFailedEventArgs e)
        {
            _log.Info($"Sending ticket '{e.TicketId}' failed.");
        }

        private void HandleTicketResponse(ITicketResponse ticket)
        {
            _log.Info($"Ticket '{ticket.TicketId}' response is {ticket.Status}. Reason={ticket.Reason?.Message}");

            if (ticket.BetDetails != null && ticket.BetDetails.Any())
            {
                foreach (var betDetail in ticket.BetDetails)
                {
                    _log.Info($"Bet decline reason: '{betDetail.Reason?.Message}'.");
                    if (betDetail.SelectionDetails != null && betDetail.SelectionDetails.Any())
                    {
                        foreach (var selectionDetail in betDetail.SelectionDetails)
                        {
                            _log.Info($"Selection decline reason: '{selectionDetail.Reason?.Message}'.");
                        }
                    }
                }
            }

            if (ticket.Status == TicketAcceptance.Accepted)
            {
                //required only if 'explicit acking' is enabled in MTS admin
                ticket.Acknowledge();

                // handle ticket response

                //if for some reason we want to cancel ticket, this is how we can do it
                var ticketCancel = _factory.CreateTicketCancelBuilder().SetTicketId(ticket.TicketId).SetCode(TicketCancellationReason.BookmakerTechnicalIssue).BuildTicket();
                _mtsSdk.SendTicket(ticketCancel);
            }
        }

        private void HandleTicketCancelResponse(ITicketCancelResponse ticket)
        {
            _log.Info($"Ticket '{ticket.TicketId}' response is {ticket.Status}. Reason={ticket.Reason?.Message}");
            if (ticket.Status == TicketCancelAcceptance.Cancelled)
            {
                //required only if 'explicit acking' is enabled in MTS admin
                ticket.Acknowledge();
            }
        }
    }
}
