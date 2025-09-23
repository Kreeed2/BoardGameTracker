using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

//var username = builder.AddParameter("username");
//var password = builder.AddParameter("password", secret: true);

var cache = builder.AddRedis("cache");

var database = builder.AddPostgres("database")
   .WithDataVolume(isReadOnly: false)
   .AddDatabase("postgresdb");

var keycloak = builder.AddKeycloak("keycloak", 8080)
    .WithDataVolume();
    //.WithRealmImport("./BoardGameTracker");

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
