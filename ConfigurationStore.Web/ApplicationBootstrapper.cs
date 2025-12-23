using ConfigurationStore.Web.Components.Account;

using LVK.Bootstrapping;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ConfigurationStore.Web;

public class ApplicationBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostApplicationBuilder builder)
    {
        builder.Bootstrap(new Core.ModuleBootstrapper());
        builder.Bootstrap(new Data.ModuleBootstrapper());
        builder.Bootstrap(new Email.ModuleBootstrapper());

        builder.Services.AddTransient<IHostInitializer<WebApplication>, ApplicationInitializer>();

        // Add services to the container.
        builder.Services.AddRazorComponents().AddInteractiveServerComponents();

        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<IdentityRedirectManager>();
        builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        }).AddIdentityCookies();

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    }
}