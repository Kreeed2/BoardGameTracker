using BoardGameTracker.ApiService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.  
builder.AddServiceDefaults();

// Add services to the container.  
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi  
builder.Services.AddOpenApi();

// Add PostgreSQL database context  
builder.AddNpgsqlDbContext<BoardGameTrackerDbContext>("postgresdb");

builder.Services.AddAuthentication()
                .AddKeycloakJwtBearer(
                    serviceName: "keycloak",
                    realm: "BoardGameTracker",
                    options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.Audience = "boardgame.api";
                    });

var app = builder.Build();

// Ensure database migrations are applied  
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BoardGameTrackerDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.  
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapDefaultEndpoints();

app.Run();