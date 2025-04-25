var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis(OutputCache);

var apiService = builder.AddProject<Projects.BlogApp_Api>(Api)
		.WithSwaggerUi()
		.WithScalar();

var mongoServer = builder.AddMongoDB(ServerName)
		.WithLifetime(ContainerLifetime.Persistent)
		.WithMongoExpress();

var mongoDb = mongoServer.AddDatabase(DatabaseName);

builder.Build().Run();
