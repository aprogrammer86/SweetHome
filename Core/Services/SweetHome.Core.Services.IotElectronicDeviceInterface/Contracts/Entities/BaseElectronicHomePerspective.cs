using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetHome.Core.Services.IotElectronicDeviceInterface.Contracts.Entities
{
    public abstract class BaseElectronicHomePerspective
    {
        public int HomeId { get; set; }
        public List<BaseEDevice> Devices { get; set; } = null!;
    }
}
