using EventBus.Contracts;
using EventBus.RabbitMq.Contracts;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace EventBus.RabbitMq
{
    public record SubscribeInfo(Type Event, Type EventHandler);
    public class EventBusRabbitMq : IEventBus, IDisposable
    {
        private readonly Dictionary<string, List<SubscribeInfo>> _subscribers = new();
        const string EXCHANGE_NAME = "sweet_home_bus";

        private readonly IRabbitMqConnection<IModel> _connection;
        private readonly string _queueName;
        private readonly IServiceProvider _sp;
        private readonly IModel _consumerChannel;

        public EventBusRabbitMq(IRabbitMqConnection<IModel> connection, string queueName, IServiceProvider sp)
        {
            _connection = connection;
            _queueName = queueName;
            _sp = sp;
            _consumerChannel = CreateConsumerChannel();
        }
        public void Publish(IEvent @event)
        {
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }
            var eventName = @event.GetType().Name;
            var channel = _connection.CreateChannel();
            channel.ExchangeDeclare(
                exchange: EXCHANGE_NAME,
                type: ExchangeType.Direct);

            var properties = channel.CreateBasicProperties();

            properties.DeliveryMode = 2;
            channel.BasicPublish(
                exchange: EXCHANGE_NAME,
                routingKey: eventName,
                mandatory: true,
                basicProperties: properties,
                body: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event, @event.GetType(), new JsonSerializerOptions { WriteIndented = true })));
            //body: Encoding.UTF8.GetBytes($"Publish: {eventName}"));

            Console.WriteLine($"Publish: {eventName}");
        }

        public void Subscribe<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>
        {
            var consumer = new AsyncEventingBasicConsumer(_consumerChannel);
            consumer.Received += Consumer_Received;

            var eventName = typeof(TEvent).Name;

            if (!_subscribers.ContainsKey(eventName))
            {
                _subscribers[eventName] = new List<SubscribeInfo>();
            }
            _subscribers[eventName].Add(new SubscribeInfo(typeof(TEvent), typeof(THandler)));

            _consumerChannel.QueueBind(queue: _queueName, exchange: EXCHANGE_NAME, routingKey: eventName, null);
            _consumerChannel.BasicConsume(_queueName, false, consumer);
        }

        private IModel CreateConsumerChannel()
        {
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }

            var channel = _connection.CreateChannel();
            channel.ExchangeDeclare(exchange: EXCHANGE_NAME, type: ExchangeType.Direct);
            channel.QueueDeclare(queue: _queueName,
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            return channel;
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            var eventName = @event.RoutingKey;
            string message = Encoding.UTF8.GetString(@event.Body.ToArray());
            Console.WriteLine($"Consumed: {eventName}");

            await HandleEvent(eventName, message);

            _consumerChannel.BasicAck(@event.DeliveryTag, false);

            //await Task.Yield();
        }

        private async Task HandleEvent(string eventName, string message)
        {
            if (_subscribers.ContainsKey(eventName))
            {
                var subscribes = _subscribers[eventName];
                foreach (var item in subscribes)
                {
                    var ctors = item.EventHandler.GetConstructors();
                    var count = ctors.FirstOrDefault()!.GetParameters();//.Select(c=> c.;

                    object e = JsonSerializer.Deserialize(message, item.Event, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
                    object handler = ActivatorUtilities.CreateInstance(_sp, item.EventHandler);
                    MethodInfo methodInfo = item.EventHandler.GetMethod("Handle")!;
                    await (Task)methodInfo.Invoke(handler, new object[] { e })!;
                }
            }
        }

        public void Unsubscribe<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }
            _subscribers.Clear();
        }
    }
}