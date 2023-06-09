using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetHome.Core.Services.IotHomeDeviceInterface.Contracts.Commands
{
    public abstract record BaseHomeCommand
    {
        protected BaseHomeCommand(int homeId)
        {
            HomeId = homeId;
        }

        public int HomeId { get; }

    }
}
