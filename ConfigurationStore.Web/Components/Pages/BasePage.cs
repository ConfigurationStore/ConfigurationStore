using JetBrains.Annotations;

using Microsoft.AspNetCore.Components;

using Radzen;

namespace ConfigurationStore.Web.Components.Pages;

public abstract class BasePage : ComponentBase
{
    [CascadingParameter]
    public Action<string>? SetPageTitle { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    public DialogService DialogService { get; set; } = null!;

    [Inject]
    public NotificationService NotificationService { get; set; } = null!;

    protected abstract string PageTitle { get; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        UpdatePageTitle();
    }

    protected void UpdatePageTitle()
    {
        SetPageTitle?.Invoke(PageTitle);
    }
}