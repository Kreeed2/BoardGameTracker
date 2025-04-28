using BoardGameTracker.ApiService.Model;
using Microsoft.EntityFrameworkCore;

namespace BoardGameTracker.ApiService
{
    public class BoardGameTrackerDbContext(DbContextOptions<BoardGameTrackerDbContext> options) : DbContext(options)
    {
        public required DbSet<User> Users { get; set; }
    }
}