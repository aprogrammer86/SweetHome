using SweetHome.Core.Services.IotElectronicDeviceInterface.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetHome.Core.Services.IotElectronicDeviceInterface.TeaMakers.Entities
{
    public class TeaMaker : BaseEDevice
    {
        public float Current { get; set; }
    }
}
