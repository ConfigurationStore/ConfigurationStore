using LVK.Bootstrapping;

using Microsoft.EntityFrameworkCore;

namespace ConfigurationStore.Data;

internal class DatabaseMigrator : IModuleInitializer
{
    private readonly IDbContextFactory<MainDbContext> _dbContextFactory;

    public DatabaseMigrator(IDbContextFactory<MainDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await using MainDbContext dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        await dbContext.Database.MigrateAsync(cancellationToken);
    }
}