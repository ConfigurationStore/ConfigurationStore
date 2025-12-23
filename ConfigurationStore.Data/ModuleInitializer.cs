using LVK.Bootstrapping;

using Microsoft.EntityFrameworkCore;

namespace ConfigurationStore.Data;

internal class ModuleInitializer : IModuleInitializer
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

    public ModuleInitializer(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await using ApplicationDbContext dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        await dbContext.Database.MigrateAsync(cancellationToken);
    }
}