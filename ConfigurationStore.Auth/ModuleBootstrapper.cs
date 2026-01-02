using LVK.Bootstrapping;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConfigurationStore.Auth;

public class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostApplicationBuilder builder)
    {
        builder.Bootstrap(new Data.ModuleBootstrapper());

        builder.Services.AddTransient<IUserManager, UserManager>();
        builder.Services.AddScoped<IAuthenticationState, AuthenticationState>();
    }
}