using Mediator;
using System.Net.Http.Json;
using UI.SpaBlazor.Pages;

namespace UI.SpaBlazor.Features.Home
{
	public class WeatherRequest : IRequest<IEnumerable<WeatherForecast>>
	{
        public const string RouteTemplate = "/api/weather";

        //public const string RouteTemplate = "sample-data/weather.json";
    }
    public class WeatherHandler : IRequestHandler<WeatherRequest, IEnumerable<WeatherForecast>>
    {
        private readonly HttpClient _client;

        public WeatherHandler(HttpClient client)
        {
            _client = client;
        }

        public async ValueTask<IEnumerable<WeatherForecast>> Handle(WeatherRequest request, CancellationToken cancellationToken)
        {
            return await _client.GetFromJsonAsync<IEnumerable<WeatherForecast>>(WeatherRequest.RouteTemplate);
        }
    }
}
