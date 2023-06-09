using SweetHome.Core.Services.IotElectronicDeviceInterface.Contracts.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetHome.Core.Services.IotElectronicDeviceInterface.TeaMakers.Commands
{
    public class TurnOn : BaseEDeviceCommand
    {
        public TurnOn(int deviceId) : base(deviceId)
        {
        }
    }
}
