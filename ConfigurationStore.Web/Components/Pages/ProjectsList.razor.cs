using ConfigurationStore.Data;
using ConfigurationStore.Web.Components.Dialogs;
using ConfigurationStore.Web.Models;

using Microsoft.EntityFrameworkCore;

using Radzen;

namespace ConfigurationStore.Web.Components.Pages;

public partial class ProjectsList
{
    private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
    protected override string PageTitle => "Projects";

    private List<ProjectsListItemModel>? _projects;

    public ProjectsList(IDbContextFactory<MainDbContext> dbContextFactory)
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
        if (AuthenticationStateProvider.AuthenticatedUser == null)
        {
            _projects = null;
            return;
        }

        await using MainDbContext dbContext = await _dbContextFactory.CreateDbContextAsync();
        int userId = AuthenticationStateProvider.AuthenticatedUser!.Id;
        List<Project> projects = await dbContext.Projects.Include(p => p.Owner)
           .Where(p => p.OwnerId == userId || p.Environments.Any(e => e.Users.Any(u => u.Id == userId)) || p.Environments.Any(e => e.UserGroups.Any(g => g.Users.Any(u => u.Id == userId))))
           .OrderBy(p => p.Name).ToListAsync();
        _projects = projects.Select(p => new ProjectsListItemModel(p)).ToList();
    }

    private async Task NewProject()
    {
        var model = new EditProjectDialogModel
        {
            AcceptButtonText = "Create",
        };
        await DialogService.OpenAsync<EditProjectDialog>("New Project", new Dictionary<string, object>() { ["model"] = model });

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