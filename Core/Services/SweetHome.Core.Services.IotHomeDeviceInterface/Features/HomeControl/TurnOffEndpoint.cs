using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using SweetHome.Core.Services.IotHomeDeviceInterface.TeaMakers.Commands;
using SweetHome.Core.Services.IotHomeDeviceInterface.TeaMakers.Events;

namespace SweetHome.Core.Services.IotHomeDeviceInterface.Features.HomeControl
{
    //[Route($"/api/teamaker")]
    public class TurnOffEndpoint : EndpointBaseSync.WithRequest<TurnOff>.WithResult<TurnedOff>
    {
        [HttpPost("/api/teamaker/turnoff")]
        public override TurnedOff Handle([FromBody] TurnOff request)
        {
            return new TurnedOff();
        }
    }
}
