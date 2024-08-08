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
	.WithEndpoint(port: 5282, targetPort: 5280, scheme: "tcp", name: "mqqtport", isExternal: true, isProxied: false)
	.WithEndpoint(port: 5283, targetPort: 5281, scheme: "tcp", name: "mqqtwebport", isExternal: true, isProxied: false);

builder.Build().Run();
