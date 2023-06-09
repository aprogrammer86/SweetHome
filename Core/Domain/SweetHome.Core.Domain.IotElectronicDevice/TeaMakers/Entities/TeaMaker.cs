using SweetHome.Core.Domain.IotElectronicDevice.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetHome.Core.Domain.IotElectronicDevice.TeaMakers.Entities
{
    public class TeaMaker : BaseEDevice
    {
        public float Current { get; set; }
    }
}
