using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var databaseName = "postgresdb";
var creationScript = $"""
    -- Create the database
    IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{databaseName}')
    BEGIN
      CREATE DATABASE {databaseName};
    END;
    GO
    """;

var database = builder.AddPostgres("database")
   .WithDataVolume(isReadOnly: false)
   .AddDatabase(databaseName)
   .WithCreationScript(creationScript);

var keycloak = builder.AddKeycloak("keycloak", 8080)
    .WithDataVolume();

var apiService = builder.AddProject<Projects.BoardGameTracker_ApiService>("apiservice")
    .WithReference(database)
    .WaitFor(database)
    .WithReference(keycloak)
    .WaitFor(keycloak);

builder.AddProject<Projects.BoardGameTracker_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(keycloak)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
