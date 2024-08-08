using MQTTnet.AspNetCore;
using TestMqttBroker;

int mqttPort = 5280;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to listen on specific ports
builder.WebHost.ConfigureKestrel(options =>
{
	// This will allow MQTT connections based on TCP (sockets)
	options.ListenAnyIP(mqttPort, listenOptions =>
	{
		listenOptions.UseMqtt();
	});

	// This will allow MQTT connections based on HTTP WebSockets with URI "0:0:0:0:5281/mqtt"
	options.ListenAnyIP(mqttPort + 1); // Default HTTP pipeline
});

builder.AddServiceDefaults();

builder.Services.AddHostedMqttServer(optionsBuilder =>
{
	optionsBuilder.WithDefaultEndpoint()
		.WithTcpKeepAliveInterval(10)
		.WithTcpKeepAliveTime(15)
		.WithKeepAlive();
});
builder.Services.AddMqttConnectionHandler();
builder.Services.AddConnections();
builder.Services.AddSingleton<MqttServerController>();
builder.Services.AddSingleton<IMqttServerAccessor, MqttServerAccessor>();

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapMqtt("/mqtt");
app.UseMqttServer(server =>
{
	var mqttController = app.Services.GetRequiredService<MqttServerController>();
	var mqttServerAccessor = app.Services.GetRequiredService<IMqttServerAccessor>();

	mqttServerAccessor.MqttServer = server;

	server.ValidatingConnectionAsync += mqttController.ValidateConnection;
	server.ClientConnectedAsync += mqttController.OnClientConnected;
	server.ClientDisconnectedAsync += mqttController.OnClientDisconnected;
	server.InterceptingSubscriptionAsync += mqttController.InterceptingSubscriptionAsync;
	server.InterceptingPublishAsync += mqttController.InterceptingPublishAsync;
});

app.Run();