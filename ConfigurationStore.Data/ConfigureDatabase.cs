using LVK.Bootstrapping;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace ConfigurationStore.Data;

internal class ConfigureDatabase : IModuleInitializer
{
    private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
    private readonly IHostEnvironment _hostEnvironment;

    public ConfigureDatabase(IDbContextFactory<MainDbContext> dbContextFactory, IHostEnvironment hostEnvironment)
    {
        _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await using MainDbContext dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        await dbContext.Database.MigrateAsync(cancellationToken);

        if (_hostEnvironment.IsDevelopment())
        {
            await InitializeDevelopmentDatabase(cancellationToken);
        }
    }

    public async Task InitializeDevelopmentDatabase(CancellationToken cancellationToken)
    {
        await using MainDbContext dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        User adminUser = await GetOrAddUser(dbContext, "admin", "Administrator");
        UserGroup administratorsGroup = await GetOrAddUserGroup(dbContext, "Administrators");
        if (!administratorsGroup.Users.Contains(adminUser))
        {
            administratorsGroup.Users.Add(adminUser);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        User developerUse = await GetOrAddUser(dbContext, "developer", "Developer");
        UserGroup developersGroup = await GetOrAddUserGroup(dbContext, "Developers");
        if (!developersGroup.Users.Contains(developerUse))
        {
            developersGroup.Users.Add(developerUse);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task<UserGroup> GetOrAddUserGroup(MainDbContext dbContext, string name)
    {
        UserGroup? administrators = await dbContext.UserGroups.Include(ug => ug.Users).FirstOrDefaultAsync(u => u.Name == name);
        if (administrators != null)
        {
            return administrators;
        }

        administrators = new UserGroup
        {
            Name = name,
        };
        dbContext.UserGroups.Add(administrators);
        await dbContext.SaveChangesAsync();
        return administrators;
    }

    private async Task<User> GetOrAddUser(MainDbContext dbContext, string username, string displayName)
    {
        User? user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user != null)
        {
            return user;
        }

        user = new User
        {
            Username = username,
            DisplayName = displayName,
            PasswordHash = "==",
        };
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        return user;
    }
}