using LVK.Bootstrapping;

using Microsoft.EntityFrameworkCore;

namespace ConfigurationStore.Data;

internal class DevelopmentDatabase : IModuleInitializer
{
    private readonly IDbContextFactory<MainDbContext> _dbContextFactory;

    public DevelopmentDatabase(IDbContextFactory<MainDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await using MainDbContext dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        User adminUser = await GetOrAddAdminUser(dbContext);
        UserGroup administratorsGroup = await GetOrAddAdministratorsGroup(dbContext);
        if (!administratorsGroup.Users.Contains(adminUser))
        {
            administratorsGroup.Users.Add(adminUser);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task<UserGroup> GetOrAddAdministratorsGroup(MainDbContext dbContext)
    {
        UserGroup? administrators = await dbContext.UserGroups.Include(ug => ug.Users).FirstOrDefaultAsync(u => u.Name == "Administrators");
        if (administrators != null)
        {
            return administrators;
        }

        administrators = new UserGroup
        {
            Name = "Administrators",
        };
        dbContext.UserGroups.Add(administrators);
        await dbContext.SaveChangesAsync();
        return administrators;
    }

    private async Task<User> GetOrAddAdminUser(MainDbContext dbContext)
    {
        User? admin = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == "admin");
        if (admin != null)
        {
            return admin;
        }

        admin = new User
        {
            Username = "admin",
            DisplayName = "Administrator",
            PasswordHash = "==",
        };
        dbContext.Users.Add(admin);
        await dbContext.SaveChangesAsync();
        return admin;
    }
}