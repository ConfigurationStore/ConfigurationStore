using ConfigurationStore.Data;
using ConfigurationStore.Web.Components.Dialogs;

using Microsoft.EntityFrameworkCore;

using Radzen;

namespace ConfigurationStore.Web.Components.Pages;

public partial class Projects
{
    private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
    protected override string PageTitle => "Projects";

    private List<ProjectsModel>? _projects;

    public Projects(IDbContextFactory<MainDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        await UpdateProjectList();
    }

    private async Task UpdateProjectList()
    {
        await using MainDbContext dbContext = await _dbContextFactory.CreateDbContextAsync();
        List<Project> projects = await dbContext.Projects.Include(p => p.Owner).OrderBy(p => p.Name).ToListAsync();
        _projects = projects.Select(p => new ProjectsModel(p)).ToList();
    }

    private async Task NewProject()
    {
        var model = new NewProjectDialogModel();
        await DialogService.OpenAsync<NewProjectDialog>("New Project", new Dictionary<string, object>() { ["model"] = model });

        if (model.Accepted)
        {
            await using MainDbContext dbContext = await _dbContextFactory.CreateDbContextAsync();
            dbContext.Projects.Add(new Project
            {
                Name = model.ProjectName.Trim(), OwnerId = AuthenticationStateProvider.AuthenticatedUser!.Id,
            });

            try
            {
                await dbContext.SaveChangesAsync();
                await UpdateProjectList();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, summary: "Unable to create project", detail: ex.Message, duration: 4000);
            }
        }
    }
}