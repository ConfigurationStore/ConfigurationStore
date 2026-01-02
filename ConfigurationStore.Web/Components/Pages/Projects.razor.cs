using Microsoft.AspNetCore.Components;

namespace ConfigurationStore.Web.Components.Pages;

public partial class Projects : ComponentBase
{
    [CascadingParameter]
    public Action<string>? SetPageTitle { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        SetPageTitle?.Invoke("Projects");
    }
}