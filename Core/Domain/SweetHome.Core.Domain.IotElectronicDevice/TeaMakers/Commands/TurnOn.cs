using SweetHome.Core.Domain.IotElectronicDevice.Contracts.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetHome.Core.Domain.IotElectronicDevice.TeaMakers.Commands
{
    public class TurnOn : BaseEDeviceCommand
    {
        public TurnOn(int deviceId) : base(deviceId)
        {
        }
    }
}
