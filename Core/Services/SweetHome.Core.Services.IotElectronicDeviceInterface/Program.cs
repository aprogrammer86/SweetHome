using EventBus.Contracts;
using EventBus.RabbitMq;
using EventBus.RabbitMq.Contracts;
using MQTTnet.Client;
using RabbitMQ.Client;
using SweetHome.Core.Services.IotElectronicDeviceInterface.ElectronicHomes.Commands;
using SweetHome.Core.Services.IotElectronicDeviceInterface.TeaMakers.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IRabbitMqConnection<IModel>>(sp =>
{
    IConnectionFactory factory = new ConnectionFactory()
    {
        DispatchConsumersAsync = true,
        HostName = "localhost",
        Port = 2672,
        UserName = "admin",
        Password = "123456"
    };
    var logger = sp.GetRequiredService<ILogger<DefaultRabbitMqConnection>>();

    return new DefaultRabbitMqConnection(factory, logger);
});

builder.Services.AddSingleton<IEventBus>(sp =>
{
    var connection = sp.GetRequiredService<IRabbitMqConnection<IModel>>();
    var queueName = "electronic_home";
    return new EventBusRabbitMq(connection, queueName, sp);
});

builder.Services.AddSingleton<IRabbitMqConnection<IMqttClient>>(sp =>
{
    var options = new MqttClientOptionsBuilder()
                            .WithClientId(Guid.NewGuid().ToString())
                            .WithTcpServer("localhost", 28833)
                            .WithCredentials("admin", "123456")
                            .WithCleanSession()
                            .WithWillRetain(false)
                            .Build();
    var logger = sp.GetRequiredService<ILogger<MqttRabbitMqConnection>>();

    return new MqttRabbitMqConnection(options, logger);
});

builder.Services.AddSingleton<EventBusMqttRabbitMq>(sp =>
{
    var connection = sp.GetRequiredService<IRabbitMqConnection<IMqttClient>>();
    return new EventBusMqttRabbitMq(connection);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

IEventBus eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<DeviceCommand, DeviceCommandHandler>();

var mqttBus = app.Services.GetRequiredService<EventBusMqttRabbitMq>();
mqttBus.Subscribe<DeviceReplied, DeviceRepliedHandler>();

app.Run();

