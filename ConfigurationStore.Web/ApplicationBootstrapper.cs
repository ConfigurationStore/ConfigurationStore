using LVK.Bootstrapping;

using Radzen;

namespace ConfigurationStore.Web;

public class ApplicationBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostApplicationBuilder builder)
    {
        builder.Bootstrap(new Data.ModuleBootstrapper());
        builder.Bootstrap(new Auth.ModuleBootstrapper());

        builder.Services.AddRadzenComponents();

        builder.Services.AddTransient<IHostInitializer<WebApplication>, ApplicationInitializer>();

        // Add services to the container.
        builder.Services.AddRazorComponents().AddInteractiveServerComponents();
    }
}