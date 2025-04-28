using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var databaseName = "postgresdb";
var creationScript = $$"""
    -- Create the database
    CREATE DATABASE "{{databaseName}}";
    """;

var database = builder.AddPostgres("database")
   .WithDataVolume(isReadOnly: false)
   .AddDatabase(databaseName)
   .WithCreationScript(creationScript);

var apiService = builder.AddProject<Projects.BoardGameTracker_ApiService>("apiservice")
    .WithReference(database)
    .WaitFor(database);

builder.AddProject<Projects.BoardGameTracker_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
