using SweetHome.Core.Domain.IotElectronicDevice.Contracts.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetHome.Core.Domain.IotElectronicDeviceInterface.Contracts.Commands
{
    public abstract class BaseCommand
    {
        public BaseCommand(int homeId, BaseEDeviceCommand command)
        {
            HomeId = homeId;
            Command = command;
        }
        public int HomeId { get; private set; }
        public BaseEDeviceCommand Command { get; private set; } = null!;
    }
}
