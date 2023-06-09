using SweetHome.Core.Domain.IotHomeDevice.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetHome.Core.Domain.IotHomeDevice.TeaMakers.Entities
{
    public class TeaMaker : BaseHDevice
    {
        public float Current { get; set; }
    }
}
