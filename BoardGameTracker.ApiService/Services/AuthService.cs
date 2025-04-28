using System.Security.Cryptography;
using System.Text;
using BoardGameTracker.ApiService;
using Microsoft.EntityFrameworkCore;

public class AuthService
{
    private readonly BoardGameTrackerDbContext _dbContext;

    public AuthService(BoardGameTrackerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> AuthenticateUserAsync(string email, string password)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null || !VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
        {
            return null; // Authentication failed
        }
        return user; // Authentication successful
    }

    public async Task<bool> RegisterUserAsync(string name, string email, string password)
    {
        if (await _dbContext.Users.AnyAsync(u => u.Email == email))
        {
            return false; // Email already exists
        }

        var (passwordHash, passwordSalt) = HashPassword(password);

        var newUser = new User
        {
            Name = name,
            Email = email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        _dbContext.Users.Add(newUser);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    private (string Hash, string Salt) HashPassword(string password)
    {
        using var hmac = new HMACSHA256();
        var salt = Convert.ToBase64String(hmac.Key);
        var hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        return (hash, salt);
    }

    private bool VerifyPassword(string password, string storedHash, string? storedSalt)
    {
        if (storedSalt == null) return false;

        using var hmac = new HMACSHA256(Convert.FromBase64String(storedSalt));
        var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        return computedHash == storedHash;
    }
}
