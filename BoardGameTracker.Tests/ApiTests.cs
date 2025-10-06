using BoardGameTracker.ApiService.Model;
using BoardGameTracker.Shared.DataTransferObjects;
using System.Net;
using System.Net.Http.Json;

namespace BoardGameTracker.Tests;

[TestFixture]
public class ApiTests
{
    private HttpClient _httpClient;
    private IDistributedApplicationTestingBuilder _appHost;

    [SetUp]
    public async Task Setup()
    {
        _appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.BoardGameTracker_AppHost>();
    }

    [TearDown]
    public async Task TearDown()
    {
        _httpClient?.Dispose();
        if (_appHost is not null)
        {
            await _appHost.DisposeAsync();
        }
    }

    [Test]
    public async Task Get_all_games_and_return_ok()
    {
        // Arrange
        await using var app = await _appHost.BuildAsync();
        await app.StartAsync();

        // Act
        _httpClient = app.CreateHttpClient("apiservice");
        var games = await _httpClient.GetFromJsonAsync<List<Game>>("/api/Game");

        // Assert
        Assert.That(games, Is.Not.Null);
        Assert.That(games, Is.Empty);
    }

    [Test]
    public async Task Create_a_new_game_and_return_created()
    {
        // Arrange
        var game = new GameTransferObject { Name = "Catan" };
        await using var app = await _appHost.BuildAsync();
        await app.StartAsync();
        _httpClient = app.CreateHttpClient("apiservice");

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/Game", game);
        var new_game = await response.Content.ReadFromJsonAsync<Game>();

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(new_game, Is.Not.Null);
            Assert.That(new_game!.Name, Is.EqualTo("Catan"));
        }

        // Cleanup
        await _httpClient.DeleteAsync($"/api/Game/{new_game.GameId}");
    }

    [Test]
    public async Task Get_a_game_by_id_and_return_ok()
    {
        // Arrange
        var game = new GameTransferObject { Name = "Catan" };
        await using var app = await _appHost.BuildAsync();
        await app.StartAsync();
        _httpClient = app.CreateHttpClient("apiservice");
        var response = await _httpClient.PostAsJsonAsync("/api/Game", game);
        var new_game = await response.Content.ReadFromJsonAsync<Game>();

        // Act
        var created_game = await _httpClient.GetFromJsonAsync<Game>($"/api/Game/{new_game.GameId}");

        // Assert
        Assert.That(created_game, Is.Not.Null);
        Assert.That(created_game.Name, Is.EqualTo("Catan"));

        // Cleanup
        await _httpClient.DeleteAsync($"/api/Game/{new_game.GameId}");
    }

    [Test]
    public async Task Update_a_game_and_return_ok()
    {
        // Arrange
        var game = new GameTransferObject { Name = "Catan" };
        await using var app = await _appHost.BuildAsync();
        await app.StartAsync();
        _httpClient = app.CreateHttpClient("apiservice");
        var response = await _httpClient.PostAsJsonAsync("/api/Game", game);
        var new_game = await response.Content.ReadFromJsonAsync<Game>();
        var updated_game = new GameTransferObject { Name = "Ticket to Ride" };

        // Act
        var update_response = await _httpClient.PutAsJsonAsync($"/api/Game/{new_game.GameId}", updated_game);
        var fetched_game = await _httpClient.GetFromJsonAsync<Game>($"/api/Game/{new_game.GameId}");

        // Assert
        Assert.That(update_response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(fetched_game.Name, Is.EqualTo("Ticket to Ride"));

        // Cleanup
        await _httpClient.DeleteAsync($"/api/Game/{new_game.GameId}");
    }

    [Test]
    public async Task Delete_a_game_and_return_ok()
    {
        // Arrange
        var game = new GameTransferObject { Name = "Catan" };
        await using var app = await _appHost.BuildAsync();
        await app.StartAsync();
        _httpClient = app.CreateHttpClient("apiservice");
        var response = await _httpClient.PostAsJsonAsync("/api/Game", game);
        var new_game = await response.Content.ReadFromJsonAsync<Game>();

        // Act
        var delete_response = await _httpClient.DeleteAsync($"/api/Game/{new_game.GameId}");
        var get_response = await _httpClient.GetAsync($"/api/Game/{new_game.GameId}");

        // Assert
        Assert.That(delete_response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(get_response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}
