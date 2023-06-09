using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UI.SpaBlazor.Contracts.Commands;

namespace UI.SpaBlazor.TeaMakers.Commands
{
    public record TurnOn : BaseHDeviceCommand
    {
        public TurnOn(int deviceId) : base(deviceId)
        {
        }
    }
}
