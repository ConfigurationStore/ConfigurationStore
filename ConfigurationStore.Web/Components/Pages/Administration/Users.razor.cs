using Microsoft.AspNetCore.Components;

namespace ConfigurationStore.Web.Components.Pages.Administration;

public partial class Users : ComponentBase
{
    [CascadingParameter]
    public Action<string>? SetPageTitle { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        SetPageTitle?.Invoke("Users");
    }
}