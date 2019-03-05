/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Diagnostics.Contracts;
using Sportradar.MTS.SDK.Entities.Builders;
using Sportradar.MTS.SDK.Entities.EventArguments;
using Sportradar.MTS.SDK.Entities.Interfaces;

namespace Sportradar.MTS.SDK.API.Contracts
{
    [ContractClassFor(typeof(IMtsSdk))]
    internal abstract class MtsSdkContract : IMtsSdk
    {
        public event EventHandler<TicketResponseReceivedEventArgs> TicketResponseReceived;

        public event EventHandler<TicketMessageEventArgs> TicketResponseTimedOut;

        public event EventHandler<UnparsableMessageEventArgs> UnparsableTicketResponseReceived;

        public event EventHandler<TicketSendFailedEventArgs> SendTicketFailed;

        [Pure]
        public IBuilderFactory BuilderFactory
        {
            get
            {
                Contract.Ensures(Contract.Result<IBuilderFactory>() != null);
                return Contract.Result<IBuilderFactory>();
            }
        }

        public abstract bool IsOpened { get; }

        public abstract void Dispose();

        public abstract void Open();

        public abstract void Close();

        public void SendTicket(ISdkTicket ticket)
        {
            Contract.Requires(ticket != null);
        }

        public ITicketResponse SendTicketBlocking(ITicket ticket)
        {
            Contract.Requires(ticket != null);
            Contract.Ensures(Contract.Result<ITicketResponse>() != null);
            return Contract.Result<ITicketResponse>();
        }

        public ITicketCancelResponse SendTicketCancelBlocking(ITicketCancel ticket)
        {
            Contract.Requires(ticket != null);
            Contract.Ensures(Contract.Result<ITicketCancelResponse>() != null);
            return Contract.Result<ITicketCancelResponse>();
        }

        public ITicketCashoutResponse SendTicketCashoutBlocking(ITicketCashout ticket)
        {
            Contract.Requires(ticket != null);
            Contract.Ensures(Contract.Result<ITicketCashoutResponse>() != null);
            return Contract.Result<ITicketCashoutResponse>();
        }

        public IMtsClientApi ClientApi => Contract.Result<IMtsClientApi>();

        public ITicketNonSrSettleResponse SendTicketNonSrSettleBlocking(ITicketNonSrSettle ticket)
        {
            Contract.Requires(ticket != null);
            Contract.Ensures(Contract.Result<ITicketNonSrSettleResponse>() != null);
            return Contract.Result<ITicketNonSrSettleResponse>();
        }
    }
}
