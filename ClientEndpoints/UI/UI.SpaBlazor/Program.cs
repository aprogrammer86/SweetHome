using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using UI.SpaBlazor;
using UI.SpaBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<IHomeControl, HomeControl>();
//builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7157") });
//builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient("HomeDeviceControl", client => client.BaseAddress = new Uri("https://localhost:7157"));
builder.Services.AddMediator();
//builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(Program).Assembly));

await builder.Build().RunAsync();