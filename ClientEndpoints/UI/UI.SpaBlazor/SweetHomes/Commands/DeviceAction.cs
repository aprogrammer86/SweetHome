using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UI.SpaBlazor.Contracts.Commands;

namespace UI.SpaBlazor.SweetHomes.Commands
{
    public record DeviceAction : BaseHomeCommand
    {
        public const string GatewayRouteTemplate = "/home/deviceaction";

        [JsonConstructor]
        public DeviceAction(int homeId, string command, int deviceId) : base(homeId)
        {
            Command = command;
            DeviceId = deviceId;
        }
        public DeviceAction(int homeId, BaseHDeviceCommand command) : base(homeId)
        {
            DeviceId = command.DeviceId;
            Command = command.GetType().Name;
        }

        public int DeviceId { get; }
        public string Command { get; }

    }
}
