using SweetHome.Core.Domain.IotHomeDevice.Contracts.Commands;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace SweetHome.Core.Domain.IotHomeDevice.TeaMakers.Commands
{
    public record TurnOff : BaseHDeviceCommand
    {
        public TurnOff(int deviceId) : base(deviceId)
        {
        }
    }
}
