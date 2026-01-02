using LVK.Bootstrapping;

using Radzen;

namespace ConfigurationStore.Web;

public class ApplicationBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostApplicationBuilder builder)
    {
        builder.Bootstrap(new Data.ModuleBootstrapper());
        builder.Bootstrap(new Auth.ModuleBootstrapper());
        builder.Bootstrap(new Api.ModuleBootstrapper());

        builder.Services.AddRadzenComponents();

        builder.Services.AddTransient<IHostInitializer<WebApplication>, ApplicationInitializer>();

        builder.Services.AddRazorComponents().AddInteractiveServerComponents();
    }
}