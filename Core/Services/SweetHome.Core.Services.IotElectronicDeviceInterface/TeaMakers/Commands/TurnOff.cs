using SweetHome.Core.Services.IotElectronicDeviceInterface.Contracts.Commands;

namespace SweetHome.Core.Services.IotElectronicDeviceInterface.TeaMakers.Commands
{
    public class TurnOff : BaseEDeviceCommand
    {
        public TurnOff(int deviceId) : base(deviceId)
        {
        }
    }
}
