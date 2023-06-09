using EventBus.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetHome.Core.Services.IotHomeDeviceInterface.Contracts.Events
{
    public class BaseHomeEvent : IEvent
    {
        public int HomeId { get; set; }
    }
}
