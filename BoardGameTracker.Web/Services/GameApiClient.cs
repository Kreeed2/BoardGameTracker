using BoardGameTracker.Shared.DataTransferObjects;

namespace BoardGameTracker.Web.Services
{
    public interface IGameApiClient
    {
        Task<IEnumerable<GameTransferObject>> GetGamesAsync(int maxItems = 10, CancellationToken cancellationToken = default);
        Task<GameTransferObject> GetGameByIdAsync(int gameId, CancellationToken cancellationToken = default);
        Task<GameTransferObject> CreateGameAsync(GameTransferObject game);
        Task<GameTransferObject> UpdateGameAsync(GameTransferObject game);
        Task DeleteGameAsync(int gameId);
    }

    public class GameApiClient(HttpClient httpClient) : IGameApiClient
    {
        public async Task<GameTransferObject> CreateGameAsync(GameTransferObject game)
        {
            var response = await httpClient.PostAsJsonAsync("/api/Game", game);
            response.EnsureSuccessStatusCode();

            var createdGame = await response.Content.ReadFromJsonAsync<GameTransferObject>();
            if (createdGame is null)
            {
                throw new InvalidOperationException("Failed to deserialize the created game.");
            }

            return createdGame;
        }

        public Task DeleteGameAsync(int gameId)
        {
            throw new NotImplementedException();
        }

        public async Task<GameTransferObject> GetGameByIdAsync(int gameId, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.GetAsync($"/api/Game/{gameId}", cancellationToken);
            response.EnsureSuccessStatusCode();

            var game = await response.Content.ReadFromJsonAsync<GameTransferObject>(cancellationToken: cancellationToken);
            if (game is null)
            {
                throw new InvalidOperationException($"Game with ID {gameId} could not be found or deserialized.");
            }

            return game;
        }

        public async Task<IEnumerable<GameTransferObject>> GetGamesAsync(int maxItems = 10, CancellationToken cancellationToken = default)
        {
            List<GameTransferObject>? games = null;

            await foreach (var game in httpClient.GetFromJsonAsAsyncEnumerable<GameTransferObject>("/api/Game", cancellationToken))
            {
                if (games?.Count >= maxItems)
                {
                    break;
                }
                if (game is not null)
                {
                    games ??= [];
                    games.Add(game);
                }
            }

            return games ?? [];
        }

        public Task<GameTransferObject> UpdateGameAsync(GameTransferObject game)
        {
            throw new NotImplementedException();
        }
    }
}
