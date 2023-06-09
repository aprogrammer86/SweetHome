using System.Net.Http.Json;
using System.Text.Json;
using UI.SpaBlazor.Contracts.Commands;
using UI.SpaBlazor.Contracts.Events;

namespace UI.SpaBlazor.Services
{
    public class HomeControl : IHomeControl
    {
        private readonly IHttpClientFactory _factory;

        public HomeControl(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<R> SendAsync<C, R>(string uri, C command)
            where C : BaseHomeCommand
            where R : BaseHomeEvent
        {
            var http = _factory.CreateClient("HomeDeviceControl");

            var result = await http.PostAsJsonAsync<C>(uri, command);

            if (result.IsSuccessStatusCode)
            {
                R e = await result.Content.ReadFromJsonAsync<R>() ?? throw new JsonException();
                return e;
            }
            else
            {
                throw new HttpRequestException();
            }
        }
    }
}
