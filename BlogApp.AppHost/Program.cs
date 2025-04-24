using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("redis");

// Add resource of BlogApp.Api
var api = builder.AddProject<BlogApp_Api>("blog-app-api")
		.WithReference(cache);

builder.Build().Run();
