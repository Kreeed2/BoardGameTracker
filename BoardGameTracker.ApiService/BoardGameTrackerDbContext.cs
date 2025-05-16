using BoardGameTracker.ApiService.Model;
using Microsoft.EntityFrameworkCore;

namespace BoardGameTracker.ApiService
{
    public class BoardGameTrackerDbContext(DbContextOptions<BoardGameTrackerDbContext> options) : DbContext(options)
    {
        public required DbSet<Game> Games { get; set; }
        public required DbSet<Player> Players { get; set; }
        public required DbSet<Score> Scores { get; set; }
    }
}