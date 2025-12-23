using ConfigurationStore.Web.Components;

using LVK.Bootstrapping;

namespace ConfigurationStore.Web;

public class ApplicationInitializer : IHostInitializer<WebApplication>
{
    public Task InitializeAsync(WebApplication host)
    {
        // Configure the HTTP request pipeline.
        if (host.Environment.IsDevelopment())
        {
            host.UseMigrationsEndPoint();
        }
        else
        {
            host.UseExceptionHandler("/Error", createScopeForErrors: true);
        }

        host.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
        host.UseAntiforgery();

        host.MapStaticAssets();
        host.MapRazorComponents<App>().AddInteractiveServerRenderMode();

        // Add additional endpoints required by the Identity /Account Razor components.
        host.MapAdditionalIdentityEndpoints();

        return Task.CompletedTask;
    }
}