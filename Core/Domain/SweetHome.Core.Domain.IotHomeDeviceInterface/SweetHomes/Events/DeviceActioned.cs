using EventBus.Contracts;
using SweetHome.Core.Domain.IotHomeDevice.Contracts.Commands;
using SweetHome.Core.Domain.IotHomeDevice.Contracts.Events;
using SweetHome.Core.Domain.IotHomeDeviceInterface.Contracts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SweetHome.Core.Domain.IotHomeDeviceInterface.SweetHomes.Events
{
    public class DeviceActioned : BaseHomeEvent
    {
        public DeviceActioned(int deviceId, BaseHEvent result)
        {
            DeviceId = deviceId;
            Result = result;
        }

        public int DeviceId { get; private set; }
        public BaseHEvent Result { get; private set; }
    }

    public class DeviceActionedHandler : IEventHandler<DeviceActioned>
    {
        public async Task Handle(IEvent @event)
        {
            Console.WriteLine(@event);

            await Task.CompletedTask;
        }
    }
}
