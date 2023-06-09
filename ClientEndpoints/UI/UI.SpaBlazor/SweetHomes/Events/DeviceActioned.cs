using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UI.SpaBlazor.Contracts.Events;

namespace UI.SpaBlazor.SweetHomes.Events
{
    public class DeviceActioned : BaseHomeEvent
    {
        public DeviceActioned(int deviceId, BaseHEvent result)
        {
            DeviceId = deviceId;
            Result = result;
        }

        public int DeviceId { get; private set; }
        public BaseHEvent Result { get; private set; }
    }
}
