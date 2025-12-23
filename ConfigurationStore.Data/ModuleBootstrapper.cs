using LVK.Bootstrapping;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationStore.Data;

public class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostApplicationBuilder builder)
    {
        builder.Services.AddTransient<IModuleInitializer, ModuleInitializer>();

        string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContextFactory<ApplicationDbContext>(options => options.UseNpgsql(connectionString, configure =>
        {
            configure.SetPostgresVersion(new Version(18, 0));
        }));

        builder.Services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
            options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
        }).AddEntityFrameworkStores<ApplicationDbContext>().AddSignInManager().AddDefaultTokenProviders();
    }
}