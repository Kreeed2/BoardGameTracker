using Microsoft.EntityFrameworkCore;
using BoardGameTracker.ApiService;
using BoardGameTracker.ApiService.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using BoardGameTracker.Shared.DataTransferObjects;
namespace BoardGameTracker.ApiService.Services;

public static class GameEndpoints
{
    public static void MapGameEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Game").WithTags(nameof(Game));

        group.MapGet("/", async (BoardGameTrackerDbContext db) =>
        {
            return await db.Games.Select(game => new GameTransferObject
            {
                GameId = game.GameId,
                Name = game.Name,
                CreatedAt = game.CreatedAt,
                UpdatedAt = game.UpdatedAt
            }).ToListAsync();
        })
        .WithName("GetAllGames")
        .WithOpenApi();

        group.MapGet("/{id:int}", async Task<Results<Ok<GameTransferObject>, NotFound>> (int gameid, BoardGameTrackerDbContext db) =>
        {
            return await db.Games.AsNoTracking()
                .FirstOrDefaultAsync(model => model.GameId == gameid)
                is Game model
                    ? TypedResults.Ok(new GameTransferObject { Name = model.Name, GameId = model.GameId, CreatedAt = model.CreatedAt, UpdatedAt = model.UpdatedAt })
                    : TypedResults.NotFound();
        })
        .WithName("GetGameById")
        .WithOpenApi();

        group.MapPut("/{id:int}", async Task<Results<Ok, NotFound>> (int gameid, GameTransferObject game, BoardGameTrackerDbContext db) =>
        {
            var affected = await db.Games
                .Where(model => model.GameId == gameid)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Name, game.Name)
                    .SetProperty(m => m.UpdatedAt, DateTime.UtcNow)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateGame")
        .WithOpenApi()
        .RequireAuthorization();

        group.MapPost("/", async (GameTransferObject game, BoardGameTrackerDbContext db) =>
        {
            var gameTransferObject = new Game() { Name = game.Name, CreatedAt = DateTime.UtcNow };
            db.Games.Add(gameTransferObject);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Game/{game.GameId}", gameTransferObject);
        })
        .WithName("CreateGame")
        .WithOpenApi()
        .RequireAuthorization();

        group.MapDelete("/{id:int}", async Task<Results<Ok, NotFound>> (int gameid, BoardGameTrackerDbContext db) =>
        {
            var affected = await db.Games
                .Where(model => model.GameId == gameid)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteGame")
        .WithOpenApi()
        .RequireAuthorization();
    }
}
