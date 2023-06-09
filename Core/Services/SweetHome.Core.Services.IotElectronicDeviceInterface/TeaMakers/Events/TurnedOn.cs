using SweetHome.Core.Services.IotElectronicDeviceInterface.Contracts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetHome.Core.Services.IotElectronicDeviceInterface.TeaMakers.Events
{
    public class TurnedOn : BaseEEvent
    {
        public TurnedOn(int homeId, int deviceId) : base(homeId, deviceId)
        {
        }
    }
}
