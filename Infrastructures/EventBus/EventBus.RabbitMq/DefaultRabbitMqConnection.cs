using EventBus.RabbitMq.Contracts;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;

namespace EventBus.RabbitMq
{
    public class DefaultRabbitMqConnection : IRabbitMqConnection<IModel>
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<DefaultRabbitMqConnection> _logger;
        private IConnection _connection;
        private bool _isDisposed;
        private readonly object _isBusy = new();

        public DefaultRabbitMqConnection(IConnectionFactory connectionFactory, ILogger<DefaultRabbitMqConnection> logger)
        {
            var t = connectionFactory.GetType();
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public bool IsConnected => _connection is { IsOpen: true } && !_isDisposed;
        //public bool IsConnected => _connection != null && _connection.IsOpen && !_isDisposed;

        public bool TryConnect()
        {
            _logger.LogInformation("RabbitMQ client is trying to connect");

            lock (_isBusy)
            {
                _connection = _connectionFactory.CreateConnection();

                if (IsConnected)
                {
                    _connection.ConnectionBlocked += OnConnectionBlocked;
                    _connection.CallbackException += OnCallbackException;
                    _connection.ConnectionShutdown += OnConnectionShutdown;

                    _logger.LogInformation($"RabbitMQ Client connected to {_connection.Endpoint.HostName}");

                    return true;
                }
                else
                {
                    _logger.LogCritical("RabbitMQ Client could not connect");

                    return false;
                }
            }
        }

        private void OnConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            if (_isDisposed) return;

            _logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

            TryConnect();
        }

        private void OnCallbackException(object? sender, RabbitMQ.Client.Events.CallbackExceptionEventArgs e)
        {

            if (_isDisposed) return;

            _logger.LogCritical("A RabbitMQ connection throw exception. Trying to re-connect...");

            TryConnect();
        }

        private void OnConnectionBlocked(object? sender, RabbitMQ.Client.Events.ConnectionBlockedEventArgs e)
        {
            if (_isDisposed) return;

            _logger.LogWarning("A RabbitMQ connection has blocked. Trying to re-connect...");

            TryConnect();
        }

        public IModel CreateChannel()
        {
            if (!IsConnected)
                throw new InvalidOperationException("There is no RabbitMQ connection available to create model");

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            _isDisposed = true;

            try
            {
                _connection.ConnectionShutdown -= OnConnectionShutdown;
                _connection.CallbackException -= OnCallbackException;
                _connection.ConnectionBlocked -= OnConnectionBlocked;

                _connection.Dispose();
            }
            catch (IOException ex)
            {
                _logger.LogCritical($"{ex.Message}");
            }
        }
    }
}