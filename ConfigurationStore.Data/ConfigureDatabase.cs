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