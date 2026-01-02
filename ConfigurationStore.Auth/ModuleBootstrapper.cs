using LVK.Bootstrapping;

using Microsoft.AspNetCore.Components.Authorization;

namespace ConfigurationStore.Auth;

public class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostApplicationBuilder builder)
    {
        builder.Bootstrap(new LVK.Events.ModuleBootstrapper());

        builder.Bootstrap(new Data.ModuleBootstrapper());

        builder.Services.AddTransient<IUserManager, UserManager>();

        builder.Services.AddAuthorizationCore();
        builder.Services.AddScoped<ConfigurationStoreAuthenticationStateProvider>();
        builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<ConfigurationStoreAuthenticationStateProvider>());
    }
}