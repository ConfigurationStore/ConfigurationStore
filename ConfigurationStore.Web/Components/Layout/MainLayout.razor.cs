using ConfigurationStore.Auth;

using Microsoft.AspNetCore.Components;

using Radzen.Blazor;

namespace ConfigurationStore.Web.Components.Layout;

public partial class MainLayout
{
    private readonly NavigationManager _navigationManager;
    private readonly ConfigurationStoreAuthenticationStateProvider _authenticationStateProvider;

    private bool _sidebarExpanded = true;
    private string _pageTitle = "???";

    public MainLayout(NavigationManager navigationManager, ConfigurationStoreAuthenticationStateProvider authenticationStateProvider)
    {
        _navigationManager = navigationManager ?? throw new ArgumentNullException(nameof(navigationManager));
        _authenticationStateProvider = authenticationStateProvider ?? throw new ArgumentNullException(nameof(authenticationStateProvider));
    }

    private void SetPageTitle(string title)
    {
        _pageTitle = title;
        StateHasChanged();
    }

    private Task ProfileButtonClick(RadzenSplitButtonItem? arg)
    {
        switch (arg?.Value ?? "profile")
        {
            case "profile":
                _navigationManager.NavigateTo("/profile");
                break;

            case "logout":
                _authenticationStateProvider.SetLoggedOut();
                break;
        }

        return Task.CompletedTask;
    }
}