var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis(OutputCache);

var mongoServer = builder.AddMongoDB(ServerName)
		.WithLifetime(ContainerLifetime.Persistent)
		.WithMongoExpress();

var mongoDb = mongoServer.AddDatabase(DatabaseName);

builder.Build().Run();
