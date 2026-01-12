using ConfigurationStore.Data;

namespace ConfigurationStore.Web.Components.Pages;

public class ProjectEnvironmentsListItemModel
{
    private readonly ProjectEnvironment _environment;

    public ProjectEnvironmentsListItemModel(ProjectEnvironment environment)
    {
        _environment = environment ?? throw new ArgumentNullException(nameof(environment));
    }

    public string Name => _environment.Name;
}