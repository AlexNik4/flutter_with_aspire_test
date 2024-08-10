var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.AspireApp2_ApiService>("apiservice")
	.WithExternalHttpEndpoints();

builder.AddProject<Projects.AspireApp2_Web>("webfrontend")
	.WithExternalHttpEndpoints()
	.WithReference(cache)
	.WithReference(apiService);

builder.AddProject<Projects.WebApplication1>("flutterweb")
	.WithExternalHttpEndpoints()
	.WithReference(apiService);

builder.AddProject<Projects.TestMqttBroker>("testmqttbroker")
	.WithEndpoint(targetPort: 5280, scheme: "tcp", env: "mqttportenv", isExternal: true)
	.WithEndpoint(targetPort: 5281, scheme: "tcp", env: "mqttwebportenv", isExternal: false);

builder.Build().Run();
