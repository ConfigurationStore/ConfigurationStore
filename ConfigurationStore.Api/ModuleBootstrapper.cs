using LVK.Bootstrapping;

namespace ConfigurationStore.Api;

public class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostApplicationBuilder builder)
    {
        builder.Services.AddControllers().AddApplicationPart(typeof(ModuleBootstrapper).Assembly);

        builder.Services.AddTransient<IHostInitializer<WebApplication>, ModuleInitializer>();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }
}