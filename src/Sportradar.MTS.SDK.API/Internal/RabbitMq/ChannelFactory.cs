/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using log4net;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using Sportradar.MTS.SDK.Common.Log;

namespace Sportradar.MTS.SDK.API.Internal.RabbitMq
{
    /// <summary>
    /// Represents a factory used to construct <see cref="IModel"/> instances representing channels to the broker
    /// </summary>
    /// <seealso cref="IChannelFactory" />
    /// <seealso cref="System.IDisposable" />
    public class ChannelFactory : IChannelFactory, IDisposable
    {
        private static readonly ILog ExecutionLog = SdkLoggerFactory.GetLogger(typeof(ChannelFactory));

        /// <summary>
        /// The <see cref="IConnectionFactory"/> used to construct connections to the broker
        /// </summary>
        private readonly IConnectionFactory _connectionFactory;

        /// <summary>
        /// The object used to ensure thread safety
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// The <see cref="IConnection"/> representing connection to the broker
        /// </summary>
        private IConnection _connection;

        /// <summary>
        /// Value indicating whether the current instance has been disposed
        /// </summary>
        private bool _disposed;

        private readonly Dictionary<int, ChannelWrapper> _models = new Dictionary<int, ChannelWrapper>();
        private bool _connectionIsShutdown;
        private DateTime _connectionStatusChange = DateTime.MinValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelFactory"/> class
        /// </summary>
        /// <param name="connectionFactory">The connection factory</param>
        public ChannelFactory(IConnectionFactory connectionFactory)
        {
            Contract.Requires(connectionFactory != null);

            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources</param>
        protected void Dispose(bool disposing)
        {
            if (_disposed || !disposing)
            {
                return;
            }

            lock (_lock)
            {
                _disposed = true;
                RemoveChannels();
                RemoveConnection();
                _connection?.Dispose();
            }
        }

        private void CreateConnection()
        {
            ExecutionLog.Debug("Creating connection ...");
            var connection = _connectionFactory.CreateConnection();
            connection.CallbackException += ConnectionOnCallbackException;
            connection.ConnectionBlocked += ConnectionOnConnectionBlocked;
            connection.ConnectionShutdown += ConnectionOnConnectionShutdown;
            connection.ConnectionUnblocked += ConnectionOnConnectionUnblocked;
            _connection = connection;
            _connectionIsShutdown = false;
            _connectionStatusChange = DateTime.Now;
            ExecutionLog.Debug("Creating connection ... finished.");
        }

        private void RemoveConnection()
        {
            if (_connection != null)
            {
                ExecutionLog.Debug("Removing connection ...");
                if (_connection.IsOpen)
                {
                    _connection.Close();
                }
                _connection.CallbackException -= ConnectionOnCallbackException;
                _connection.ConnectionBlocked -= ConnectionOnConnectionBlocked;
                _connection.ConnectionShutdown -= ConnectionOnConnectionShutdown;
                _connection.ConnectionUnblocked -= ConnectionOnConnectionUnblocked;
                _connection = null;
                _connectionIsShutdown = false;
                _connectionStatusChange = DateTime.MinValue;
                ExecutionLog.Debug("Removing connection ... finished.");
            }
        }

        private void ConnectionOnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            var reason = _connection?.CloseReason?.Cause ?? $"{_connection?.CloseReason?.ReplyCode}-{_connection?.CloseReason?.ReplyText}";
            ExecutionLog.Info($"Connection invoked CallbackException. ClientName: {_connection?.ClientProvidedName} and close reason: {reason}.");
        }

        private void ConnectionOnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            var reason = _connection?.CloseReason?.Cause ?? $"{_connection?.CloseReason?.ReplyCode}-{_connection?.CloseReason?.ReplyText}";
            ExecutionLog.Info($"Connection invoked ConnectionBlocked. ClientName: {_connection?.ClientProvidedName} and close reason: {reason}.");
        }

        private void ConnectionOnConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            var reason = _connection?.CloseReason?.Cause ?? $"{_connection?.CloseReason?.ReplyCode}-{_connection?.CloseReason?.ReplyText}";
            ExecutionLog.Info($"Connection invoked ConnectionShutdown. ClientName: {_connection?.ClientProvidedName} and close reason: {reason}.");

            _connectionIsShutdown = true;
            _connectionStatusChange = DateTime.Now;

            foreach (var model in _models)
            {
                model.Value.MarkedForDeletion = true;
            }
        }

        private void ConnectionOnConnectionUnblocked(object sender, EventArgs e)
        {
            var reason = _connection?.CloseReason?.Cause ?? $"{_connection?.CloseReason?.ReplyCode}-{_connection?.CloseReason?.ReplyText}";
            ExecutionLog.Info($"Connection invoked ConnectionUnblocked. ClientName: {_connection?.ClientProvidedName} and close reason: {reason}.");
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets the unique id channel can use
        /// </summary>
        /// <returns>System.Int32</returns>
        public int GetUniqueId()
        {
            lock (_lock)
            {
                var r = new Random();
                var i = 1000;
                while (i > 0)
                {
                    i--;
                    var id = r.Next();
                    if (!_models.ContainsKey(id))
                    {
                        _models.Add(id, null);
                        return id;
                    }
                }
                return r.Next();
            }
        }

        /// <summary>
        /// Returns a <see cref="ChannelWrapper"/> containing a channel used to communicate with the broker
        /// </summary>
        /// <returns>a <see cref="ChannelWrapper"/> containing a channel used to communicate with the broker</returns>
        public ChannelWrapper GetChannel(int id)
        {
            lock (_lock)
            {
                if (_connection == null)
                {
                    CreateConnection();
                }
                else
                {
                    if (_connectionIsShutdown && _connection.IsOpen)
                    {
                        RemoveChannels();
                        RemoveConnection();
                        Thread.Sleep(1000);
                        CreateConnection();
                    }
                }

                //if (_connectionStatusChange > DateTime.MinValue && (DateTime.Now - _connectionStatusChange).TotalSeconds > 60 && !_connection.IsOpen)
                //{
                //    RemoveChannels();
                //    RemoveConnection();
                //    Thread.Sleep(1000);
                //    CreateConnection();
                //}

                if (!_connection.IsOpen)
                {
                    throw new ConnectFailureException("Cannot create the channel because the connection is closed.", null);
                }

                ChannelWrapper wrapper;
                if (_models.TryGetValue(id, out wrapper))
                {
                    if (wrapper != null)
                    {
                        return wrapper;
                    }
                }

                var model = _connection.CreateModel();
                wrapper = new ChannelWrapper(id, model);
                if (_models.ContainsKey(id))
                {
                    _models.Remove(id);
                    _models.Add(id, wrapper);
                    return wrapper;
                }
                _models.Add(id, wrapper);
                return wrapper;
            }
        }

        private void RemoveChannels()
        {
            var i = 100;
            while (i > 0 && _models.Count(c => c.Value.MarkedForDeletion) > 0)
            {
                i--;
                try
                {
                    var model = _models.First(f => f.Value.MarkedForDeletion);
                    RemoveChannel(model.Key);
                }
                catch (Exception)
                {
                    Thread.Sleep(500);
                }
            }
        }

        public void RemoveChannel(int id)
        {
            ChannelWrapper channelWrapper;
            if (_models.TryGetValue(id, out channelWrapper))
            {
                //ExecutionLog.Debug($"Removing channel with channelNumber: {channelWrapper.Id} started ...");
                if (channelWrapper.MarkedForDeletion)
                {
                    channelWrapper.Channel.Close();
                }
                if (channelWrapper.Channel.IsClosed)
                {
                    channelWrapper.ChannelBasicProperties = null;
                    channelWrapper.Consumer = null;
                    channelWrapper.Channel.Dispose();
                    _models.Remove(id);
                }
            }
        }
    }
}
