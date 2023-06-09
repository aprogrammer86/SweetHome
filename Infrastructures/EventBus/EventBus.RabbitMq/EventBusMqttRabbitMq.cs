using EventBus.Contracts;
using EventBus.RabbitMq.Contracts;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventBus.RabbitMq
{
    public class MqttRabbitMqConnection : IRabbitMqConnection<IMqttClient>
    {
        private readonly MqttClientOptions _options;
        private readonly ILogger<MqttRabbitMqConnection> _logger;
        private IMqttClient? _client;
        private bool _isDisposed;
        private object _isBusy = new object();
        public MqttRabbitMqConnection(MqttClientOptions options, ILogger<MqttRabbitMqConnection> logger)
        {
            _options = options;
            _logger = logger;
        }
        public bool IsConnected => _client is { IsConnected: true } && !_isDisposed;

        public IMqttClient CreateChannel()
        {
            if (_client == null)
            {
                _client = new MqttFactory().CreateMqttClient();
            }
            return _client;
        }

        public bool TryConnect()
        {
            if (IsConnected)
                return true;

            lock (_isBusy)
            {
                if (_client == null)
                    throw new NullReferenceException($"Call {nameof(CreateChannel)} method before {nameof(TryConnect)}");


                Task.Run<MqttClientConnectResult>(async () => await _client.ConnectAsync(_options)).Wait();
                

                if (IsConnected)
                {
                    _client.ConnectedAsync += _client_ConnectedAsync;
                    _client.DisconnectedAsync += _client_DisconnectedAsync;

                    _logger.LogInformation("MQTT connect to RabbitMQ.");

                    return true;
                }
                else
                {
                    _logger.LogCritical("Fail to connect MQTT on RabbitMQ.");

                    return false;
                }

            }
        }

        private Task _client_DisconnectedAsync(MqttClientDisconnectedEventArgs arg)
        {
            return Task.CompletedTask;
        }

        private Task _client_ConnectedAsync(MqttClientConnectedEventArgs arg)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            _isDisposed = true;

            _client.ConnectedAsync -= _client_ConnectedAsync;
            _client.DisconnectedAsync -= _client_DisconnectedAsync;
            Task.Run(async () => await _client.DisconnectAsync()).Wait();
            _client.Dispose();
        }
    }

    public class EventBusMqttRabbitMq : IEventBus
    {
        private readonly Dictionary<string, List<SubscribeInfo>> _subscriptions = new();

        private readonly IRabbitMqConnection<IMqttClient> _connection;
        public EventBusMqttRabbitMq(IRabbitMqConnection<IMqttClient> connection)
        {
            _connection = connection;
        }
        public void Publish(IEvent @event)
        {
            var channel = _connection.CreateChannel();

            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }
            var name = @event.GetType().Name;
            var body = JsonSerializer.Serialize(@event, @event.GetType(), new JsonSerializerOptions { WriteIndented = true });
            MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtLeastOnce;

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("server_topic")
                .WithPayload(body.ToLower())
                //.WithPayload((Stream)null)
                .WithQualityOfServiceLevel(qos)
                .WithRetainFlag(false)
                .Build();

            Task<MqttClientPublishResult>.Run(async () => await channel.PublishAsync(message)).Wait();
        }

        public void Subscribe<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>
        {
            var routingKey = "device_topic";// typeof(TEvent).Name;
            var options = new MqttClientSubscribeOptionsBuilder()
                .WithTopicFilter(t => t.WithTopic(routingKey))
                .Build();

            if (!_subscriptions.ContainsKey(routingKey))
            {
                _subscriptions[routingKey] = new List<SubscribeInfo>();
            }
            _subscriptions[routingKey].Add(new SubscribeInfo(Event: typeof(TEvent), EventHandler: typeof(THandler)));

            var channel = _connection.CreateChannel();
            channel.ApplicationMessageReceivedAsync += Channel_ApplicationMessageReceivedAsync;
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }
            Task.Run(async () => await channel.SubscribeAsync(options)).Wait();
            //var t = Task<MqttClientSubscribeResult>.Run(async () => await channel.SubscribeAsync(options)).Result;
        }

        private Task Channel_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
        {
            var name = arg.ApplicationMessage.Topic;
            //var message = arg.ApplicationMessage.PayloadSegment.Array;
            var message = arg.ApplicationMessage.Payload;
            if (_subscriptions.ContainsKey(name))
            {
                foreach (var item in _subscriptions[name])
                {
                    var value = JsonSerializer.Deserialize(message, item.Event);
                    var obj = Activator.CreateInstance(item.EventHandler);
                    MethodInfo method = obj!.GetType().GetMethod("Handle")!;
                    method.Invoke(obj, new[] { value });
                }
            }
            return Task.CompletedTask;
        }

        public void Unsubscribe<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>
        {
            throw new NotImplementedException();
        }
    }
}
