using Mediator;
using System.Net.Http.Json;
using UI.SpaBlazor.Contracts.Commands;
using UI.SpaBlazor.Contracts.Events;

namespace UI.SpaBlazor.Features.DeviceControl.TeaMaker
{
    public class TeaMakerRequest : IRequest<BaseHEvent>
    {
        public const string RouteTemplate = "/api/teamker";
        public BaseHDeviceCommand Cmd { get; private set; }
        public TeaMakerRequest(BaseHDeviceCommand cmd)
        {
            Cmd = cmd;
        }
    }
    public class TeaMakerHandler : IRequestHandler<TeaMakerRequest, BaseHEvent>
    {
        private readonly IHttpClientFactory _factory;

        public TeaMakerHandler(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async ValueTask<BaseHEvent> Handle(TeaMakerRequest request, CancellationToken cancellationToken)
        {
            var http = _factory.CreateClient("HomeDeviceControl");
            var route = $"{TeaMakerRequest.RouteTemplate}/{request.Cmd.GetType().Name}";
            var result = await http.PostAsJsonAsync<TeaMakerRequest>(route, request);

            if(result.IsSuccessStatusCode)
            {
                BaseHEvent done = await result.Content.ReadFromJsonAsync<BaseHEvent>();
                return done;
            }
            else
            {
                throw new Exception("Nashod");
            }
        }
    }

}
