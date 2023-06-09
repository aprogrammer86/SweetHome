using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.WebApiCompatShim;

namespace ApiGateways.Web.IoT.Control.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ReverseProxyOption
    {
        public PathString RoutingCondition { get; set; } = null!;
        public Uri ToService { get; set; } = null!;
    }
    public class ReverseProxy
    {
        private readonly RequestDelegate _next;
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly ReverseProxyOption _option;
        public ReverseProxy(RequestDelegate next, ReverseProxyOption option)
        {
            _next = next;
            _option = option;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            Console.WriteLine("======================================");
            var requestPath = httpContext.Request.Path;

            if (requestPath.StartsWithSegments(_option.RoutingCondition))
            {
                HttpRequestMessage message = RewriteRequestUri(httpContext, _option);

                var response = await _httpClient.SendAsync(message, HttpCompletionOption.ResponseHeadersRead);

                httpContext.Response.StatusCode = (int)response.StatusCode;
                Console.WriteLine("Response Headers:");

                AddResponseHeaders(httpContext, response);

                httpContext.Response.Headers.Remove("transfer-encoding");

                await response.Content.CopyToAsync(httpContext.Response.Body);

                return;
            }

            await _next(httpContext);
        }

        private static HttpRequestMessage RewriteRequestUri(HttpContext httpContext, ReverseProxyOption options)
        {
            foreach (var item in httpContext.Request.Headers)
            {
                var values = string.Join(", ", item.Value);
                Console.WriteLine($"{item.Key} => {values}");
            }
            var message = httpContext.GetHttpRequestMessage();
            var uri = new UriBuilder(options.ToService);
            var path = message.RequestUri.AbsolutePath.Replace(options.RoutingCondition, options.ToService.AbsolutePath);
            uri.Path = path;
            message.RequestUri = uri.Uri;
            return message;
        }

        private void AddResponseHeaders(HttpContext httpContext, HttpResponseMessage response)
        {
            foreach (var item in response.Headers)
            {
                var values = string.Join(", ", item.Value);
                Console.WriteLine($"{item.Key} => {values}");
                httpContext.Response.Headers[item.Key] = item.Value.ToArray();
            }
            Console.WriteLine("Response Content Headers:");
            foreach (var item in response.Content.Headers)
            {
                var values = string.Join(", ", item.Value);
                Console.WriteLine($"{item.Key} => {values}");
                httpContext.Response.Headers[item.Key] = item.Value.ToArray();
            }
        }
    }
    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ReverseProxyMiddlewareExtensions
    {
        public static IApplicationBuilder UseReverseProxy(this IApplicationBuilder builder, ReverseProxyOption option)
        {
            return builder.UseMiddleware<ReverseProxy>(option);
        }
    }
}
