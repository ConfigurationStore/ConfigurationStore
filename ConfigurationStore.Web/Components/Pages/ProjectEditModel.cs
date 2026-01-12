namespace ConfigurationStore.Web.Components.Pages;

public class ProjectEditModel
{
    private readonly Data.Project _project;

    public ProjectEditModel(Data.Project project)
    {
        _project = project ?? throw new ArgumentNullException(nameof(project));
    }

    public string Name => _project.Name;
}