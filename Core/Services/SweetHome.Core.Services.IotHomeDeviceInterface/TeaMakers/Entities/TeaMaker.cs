using SweetHome.Core.Services.IotHomeDeviceInterface.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetHome.Core.Services.IotHomeDeviceInterface.TeaMakers.Entities
{
    public class TeaMaker : BaseHDevice
    {
        public float Current { get; set; }
    }
}
