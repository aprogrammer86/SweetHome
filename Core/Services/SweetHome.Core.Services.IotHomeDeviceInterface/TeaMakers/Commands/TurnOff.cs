using SweetHome.Core.Services.IotHomeDeviceInterface.Contracts.Commands;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace SweetHome.Core.Services.IotHomeDeviceInterface.TeaMakers.Commands
{
    public record TurnOff : BaseHDeviceCommand
    {
        public TurnOff(int deviceId) : base(deviceId)
        {
        }
    }
}
