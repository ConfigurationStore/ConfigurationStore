using Microsoft.AspNetCore.Components;

namespace ConfigurationStore.Web.Components.Pages;

public abstract class BasePage : ComponentBase
{
    [CascadingParameter]
    public Action<string>? SetPageTitle { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    protected abstract string PageTitle { get; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        SetPageTitle?.Invoke(PageTitle);
    }
}