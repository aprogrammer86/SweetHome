using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetHome.Core.Domain.IotElectronicDevice.Contracts.Commands
{
    public abstract class BaseEDeviceCommand
    {
        public BaseEDeviceCommand(int deviceId)
        {
            DeviceId = deviceId;
        }
        public int DeviceId { get; private init; }
        public string Name { get => this.GetType().Name; }

    }
}
