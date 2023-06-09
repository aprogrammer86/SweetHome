using EventBus.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SweetHome.Core.Domain.IotHomeDeviceInterface.Communication.Commands
{
    public class DeviceCommand : IEvent
    {
        public string Command { get; private init; }
        public DeviceCommand(int homeId, int deviceId, string command)
        {
            Command = command;
        }
    }
}