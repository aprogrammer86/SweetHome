using RabbitMQ.Client;

namespace EventBus.RabbitMq.Contracts
{
    public interface IRabbitMqConnection<TChannel> : IDisposable
    {
        public bool IsConnected { get; }
        bool TryConnect();
        TChannel CreateChannel();
    }
}