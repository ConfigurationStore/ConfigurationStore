using ConfigurationStore.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationStore.Auth;

internal class UserManager : IUserManager
{
    private readonly IDbContextFactory<MainDbContext> _dbContextFactory;

    public UserManager(IDbContextFactory<MainDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    public async Task<User> CreateUserAsync(string username, string displayName, string password, CancellationToken cancellationToken = default)
    {
        if (!UserValidator.IsValidUsername(username))
        {
            throw new ArgumentException("Invalid username", nameof(username));
        }

        if (!UserValidator.IsValidPassword(password))
        {
            throw new ArgumentException("Invalid password", nameof(password));
        }

        var user = new User
        {
            Username = username,
            DisplayName = !string.IsNullOrWhiteSpace(displayName) ? displayName : username,
            PasswordHash = HashPassword(password),
        };

        await using MainDbContext dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<User?> TryLoginUserAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        await using MainDbContext dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        User? user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);

        if (user is null)
        {
            return null;
        }

        return VerifyPassword(password, user.PasswordHash) ? user : null;
    }

    public async Task<User> ChangePasswordAsync(string username, string oldPassword, string newPassword, CancellationToken cancellationToken = default)
    {
        if (!UserValidator.IsValidPassword(newPassword))
        {
            throw new ArgumentException("Invalid password", nameof(newPassword));
        }

        await using MainDbContext dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        User? user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);

        if (user is null)
        {
            throw new InvalidOperationException("User not found");
        }

        if (!VerifyPassword(oldPassword, user.PasswordHash))
        {
            throw new InvalidOperationException("Invalid password");
        }

        user.PasswordHash = HashPassword(newPassword);
        await dbContext.SaveChangesAsync(cancellationToken);
        return user;
    }

    private static string HashPassword(string password) => new PasswordHasher<string>().HashPassword("", password);
    private static bool VerifyPassword(string password, string hashedPassword) => new PasswordHasher<string>().VerifyHashedPassword("", hashedPassword, password) == PasswordVerificationResult.Success;
}