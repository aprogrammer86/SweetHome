using SweetHome.Core.Domain.IotElectronicDevice.Contracts.Commands;

namespace SweetHome.Core.Domain.IotElectronicDevice.TeaMakers.Commands
{
    public class TurnOff : BaseEDeviceCommand
    {
        public TurnOff(int deviceId) : base(deviceId)
        {
        }
    }
}
