using System.Security.Cryptography;
using System.Text;
using BoardGameTracker.ApiService;
using BoardGameTracker.ApiService.Model;
using BoardGameTracker.Shared.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

public class AuthService(BoardGameTrackerDbContext dbContext)
{
    public async Task<UserDto?> AuthenticateUserAsync(string email, string password)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null || !VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
        {
            return null; // Authentication failed
        }
        return new UserDto(user.Id, user.Name, user.Email, user.CreatedAt, user.UpdatedAt); // Authentication successful
    }

    public async Task<bool> RegisterUserAsync(string name, string email, string password)
    {
        if (await dbContext.Users.AnyAsync(u => u.Email == email))
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

        dbContext.Users.Add(newUser);
        await dbContext.SaveChangesAsync();
        return true;
    }

    private static (string Hash, string Salt) HashPassword(string password)
    {
        using var hmac = new HMACSHA256();
        var salt = Convert.ToBase64String(hmac.Key);
        var hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        return (hash, salt);
    }

    private static bool VerifyPassword(string password, string storedHash, string? storedSalt)
    {
        if (storedSalt == null) return false;

        using var hmac = new HMACSHA256(Convert.FromBase64String(storedSalt));
        var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        return computedHash == storedHash;
    }
}
