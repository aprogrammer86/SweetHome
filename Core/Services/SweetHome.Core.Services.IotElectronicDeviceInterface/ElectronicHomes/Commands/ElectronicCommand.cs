using SweetHome.Core.Services.IotElectronicDeviceInterface.Contracts.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetHome.Core.Services.IotElectronicDeviceInterface.ElectronicHomes.Commands
{
    public class ElectronicCommand : BaseCommand
    {
        public ElectronicCommand(int homeId, BaseEDeviceCommand command) : base(homeId, command)
        {
        }
    }
}
