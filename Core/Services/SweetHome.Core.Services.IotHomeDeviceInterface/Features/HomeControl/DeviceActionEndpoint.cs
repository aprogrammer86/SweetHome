using Ardalis.ApiEndpoints;
using EventBus.Contracts;
using Microsoft.AspNetCore.Mvc;
using SweetHome.Core.Services.IotHomeDeviceInterface.Communication.Commands;
using SweetHome.Core.Services.IotHomeDeviceInterface.SweetHomes.Commands;
using SweetHome.Core.Services.IotHomeDeviceInterface.SweetHomes.Events;
using SweetHome.Core.Services.IotHomeDeviceInterface.TeaMakers.Events;

namespace SweetHome.Core.Services.IotHomeDeviceInterface.Features.HomeControl
{

    public class DeviceActionEndpoint : EndpointBaseSync.WithRequest<DeviceAction>.WithActionResult<DeviceActioned>
    {
        private readonly IEventBus _eventBus;

        public DeviceActionEndpoint(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        [HttpPost("/api" + DeviceAction.GatewayRouteTemplate)]
        public override ActionResult<DeviceActioned> Handle([FromBody] DeviceAction request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            DeviceActioned result = request.Command switch
            {
                "TurnOn" => new DeviceActioned(request.DeviceId, new TurnedOn()),
                "TurnOff" => new DeviceActioned(request.DeviceId, new TurnedOff()),
                _ => throw new ArgumentOutOfRangeException(nameof(request))
            };
            var cmd = new DeviceCommand(request.HomeId, request.DeviceId, request.Command);
            _eventBus.Publish(cmd);

            return Ok(result);
        }


    }
}
