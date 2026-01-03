using ConfigurationStore.Auth;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace ConfigurationStore.Web.Components.Pages;

public abstract class AuthenticatedPageBase : BasePage, IDisposable
{
    [Inject]
    public ConfigurationStoreAuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        AuthenticationStateProvider.AuthenticationStateChanged += OnAuthenticationStageChanged;

        CheckAuthentication();
    }

    private void CheckAuthentication()
    {
        if (AuthenticationStateProvider.AuthenticatedUser == null)
        {
            InvokeAsync(() =>
            {
                NavigationManager.NavigateTo("/?returnUrl=" + NavigationManager.Uri);
            });
        }

    }

    protected virtual void OnAuthenticationStageChanged(Task<AuthenticationState> task)
    {
        CheckAuthentication();
    }

    public virtual void Dispose()
    {
        AuthenticationStateProvider.AuthenticationStateChanged -= OnAuthenticationStageChanged;
    }
}