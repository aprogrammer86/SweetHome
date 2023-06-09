using EventBus.Contracts;
using EventBus.RabbitMq.Contracts;
using EventBus.RabbitMq;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(opts =>
{
    opts.AddPolicy("AllowedAnyOrigin", policy =>
    {
        policy.AllowAnyOrigin();
    });
});

builder.Services.AddSingleton<IRabbitMqConnection<IModel>>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<DefaultRabbitMqConnection>>();

    RabbitMQ.Client.IConnectionFactory factory = new ConnectionFactory()
    {
        DispatchConsumersAsync = true,
        HostName = "localhost",
        Port = 2672,
        UserName = "admin",
        Password = "123456",
    };

    return new DefaultRabbitMqConnection(factory, logger);
});

builder.Services.AddSingleton<IEventBus, EventBusRabbitMq>(sp =>
{
    var connection = sp.GetRequiredService<IRabbitMqConnection<IModel>>();
    return new EventBusRabbitMq(connection, "home_device", sp);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowedAnyOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
