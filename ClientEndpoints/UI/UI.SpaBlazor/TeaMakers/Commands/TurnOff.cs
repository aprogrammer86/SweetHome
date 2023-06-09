using System.Text.Json.Serialization;
using System.Xml.Linq;
using UI.SpaBlazor.Contracts.Commands;

namespace UI.SpaBlazor.TeaMakers.Commands
{
    public record TurnOff : BaseHDeviceCommand
    {
        public TurnOff(int deviceId) : base(deviceId)
        {
        }
    }
}
