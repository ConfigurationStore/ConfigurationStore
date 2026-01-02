using LVK.Bootstrapping;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConfigurationStore.Data;

public class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostApplicationBuilder builder)
    {
        builder.Services.AddTransient<IModuleInitializer, ConfigureDatabase>();

        builder.Services.AddDbContextFactory<MainDbContext>(options =>
        {
            string connectionString = builder.Configuration.GetConnectionString("Default") ?? throw new InvalidOperationException("No connection string found for 'Default' connection");

            options.UseNpgsql(connectionString, configure => configure.SetPostgresVersion(18, 0));
        });
    }
}