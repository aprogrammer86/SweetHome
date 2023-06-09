using EventBus.Contracts;
using SweetHome.Core.Domain.IotElectronicDevice.Contracts.Events;

namespace SweetHome.Core.Domain.IotElectronicDevice.TeaMakers.Events
{
    public class DeviceReplied : BaseEEvent
    {
        public string Message { get; set; } = null!;
        public int Current { get; set; }
    }
    public class DeviceRepliedHandler : IEventHandler<DeviceReplied>
    {
        public Task Handle(IEvent @event)
        {
            Console.WriteLine(@event);
            return Task.CompletedTask;
        }
    }
}
