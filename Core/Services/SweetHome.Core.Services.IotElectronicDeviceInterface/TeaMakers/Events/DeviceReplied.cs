using EventBus.Contracts;
using SweetHome.Core.Services.IotElectronicDeviceInterface.Contracts.Events;

namespace SweetHome.Core.Services.IotElectronicDeviceInterface.TeaMakers.Events
{
    public class DeviceReplied : BaseEEvent
    {
        public DeviceReplied(int homeId, int deviceId, string message, int current) : base(homeId, deviceId)
        {
            Message = message;
            Current = current;
        }

        public string Message { get; private init; } = null!;
        public int Current { get; private init; }
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
