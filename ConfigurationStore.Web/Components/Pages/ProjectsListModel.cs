using ConfigurationStore.Data;

namespace ConfigurationStore.Web.Components.Pages;

public class ProjectsListModel
{
    private readonly Project _project;

    public ProjectsListModel(Project project)
    {
        _project = project ?? throw new ArgumentNullException(nameof(project));
    }

    public string Name => _project.Name;
    public string Owner => _project.Owner.DisplayName;
    public string ProjectUrl => $"/projects/{_project.Id}";
    public string OwnerUrl => $"/users/{_project.Owner.Id}";
}