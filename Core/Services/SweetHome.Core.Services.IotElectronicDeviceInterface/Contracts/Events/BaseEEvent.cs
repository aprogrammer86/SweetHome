using EventBus.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetHome.Core.Services.IotElectronicDeviceInterface.Contracts.Events
{
    public abstract class BaseEEvent : IEvent
    {
        protected BaseEEvent(int homeId, int deviceId)
        {
            HomeId = homeId;
            DeviceId = deviceId;
        }
        public int HomeId { get; private init; }
        public int DeviceId { get; private init; }

    }
}
