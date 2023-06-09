using EventBus.Contracts;
using EventBus.RabbitMq;
using SweetHome.Core.Services.IotElectronicDeviceInterface.Contracts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SweetHome.Core.Services.IotElectronicDeviceInterface.ElectronicHomes.Commands
{
    public class DeviceCommand : BaseEEvent
    {
        public string Command { get; private init; }
        public DeviceCommand(int homeId, int deviceId, string command) : base(homeId, deviceId)
        {
            Command = command;
        }
    }

    public class DeviceCommandHandler : IEventHandler<DeviceCommand>
    {
        private readonly EventBusMqttRabbitMq _mqttBus;

        public DeviceCommandHandler(EventBusMqttRabbitMq mqttBus)
        {
            _mqttBus = mqttBus;
        }

        public async Task Handle(IEvent @event)
        {
            Console.WriteLine(@event);
            _mqttBus.Publish(@event);

            await Task.CompletedTask;
        }
    }
}