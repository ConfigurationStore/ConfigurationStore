using LVK.Bootstrapping;

namespace ConfigurationStore.Api;

public class ModuleInitializer : IHostInitializer<WebApplication>
{
    public Task InitializeAsync(WebApplication host)
    {
        host.MapControllers();

        host.UseSwagger();
        host.UseSwaggerUI();

        return Task.CompletedTask;
    }
}