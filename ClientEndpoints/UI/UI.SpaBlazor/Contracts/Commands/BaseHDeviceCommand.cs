using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UI.SpaBlazor.Contracts.Commands
{
    public record BaseHDeviceCommand
    {
        protected BaseHDeviceCommand(int deviceId)
        {
            DeviceId = deviceId;
        }
        public int DeviceId { get; }

    }
}
