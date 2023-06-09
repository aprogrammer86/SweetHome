using ApiGateways.Web.IoT.Control.Middlewares;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddTransient(c => new HttpClient
//{
//    BaseAddress = new Uri("https://localhost:7223")
//});
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowSpecificOrigins",
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:7156");
                          policy.AllowAnyOrigin();
                          policy.AllowAnyHeader();
                      });
});
//builder.Services.AddSingleton<IEventBus, EventBusRabbitMq>()
//var options = new RewriteOptions().AddRewrite("")
//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
var app = builder.Build();

app.UseCors("MyAllowSpecificOrigins");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.UseSwagger();
    //app.UseSwaggerUI();
}
var toService = new UriBuilder("https", "localhost", 7223, "/api/home");

app.UseReverseProxy(new ReverseProxyOption {RoutingCondition = "/home",  ToService = toService.Uri });

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

app.Run();
