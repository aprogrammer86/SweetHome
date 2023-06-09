using SweetHome.Core.Services.IotHomeDeviceInterface.Contracts.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SweetHome.Core.Services.IotHomeDeviceInterface.TeaMakers.Commands
{
    public record TurnOn : BaseHDeviceCommand
    {
        public TurnOn(int deviceId) : base(deviceId)
        {
        }
    }
}
