using ConfigurationStore.Data;
using ConfigurationStore.Web.Components.Dialogs;
using ConfigurationStore.Web.Models;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

using Radzen;
using Radzen.Blazor;

namespace ConfigurationStore.Web.Components.Pages;

public partial class ProjectEdit
{
    private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
    protected override string PageTitle  => _project == null ? "Unknown project" : _project.Name;

    private Project? _project;

    [Parameter]
    public int ProjectId { get; set; }

    public ProjectEdit(IDbContextFactory<MainDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await using MainDbContext dbContext = await _dbContextFactory.CreateDbContextAsync();
        _project = await LoadProject(dbContext);
        UpdatePageTitle();
    }

    private async Task DeleteProject()
    {
        bool? result = await DialogService.Confirm(
            message: ConfirmDeleteContent,
            title: "Really delete project " + _project!.Name, new ConfirmOptions
            {
                OkButtonText = "Delete",
                CloseDialogOnEsc = true,
            });

        if (result == true)
        {
            await using MainDbContext dbContext = await _dbContextFactory.CreateDbContextAsync();
            Project? project = await LoadProject(dbContext);
            if (project == null)
            {
                NotificationService.Notify(NotificationSeverity.Error, summary: "Project not found",
                    detail: $"Project {_project.Name} no longer exists", duration: 4000);

                return;
            }

            dbContext.Projects.Remove(project);
            await dbContext.SaveChangesAsync();

            NotificationService.Notify(NotificationSeverity.Success, summary: "Project " + _project.Name + " deleted successfully", duration: 4000);
            NavigationManager.NavigateTo("/projects");
        }
    }

    private async Task RenameProject()
    {
        var model = new EditProjectDialogModel
        {
            ProjectName = _project!.Name,
            AcceptButtonText = "Rename",
        };
        await DialogService.OpenAsync<EditProjectDialog>("Rename Project", new Dictionary<string, object>() { ["model"] = model });
        Console.WriteLine(model.Accepted + ": " + model.ProjectName);

        if (model.Accepted)
        {
            await using MainDbContext dbContext = await _dbContextFactory.CreateDbContextAsync();
            Project? project = await LoadProject(dbContext);
            if (project == null)
            {
                NotificationService.Notify(NotificationSeverity.Error, summary: "Project not found",
                        detail: $"Project {_project.Name} no longer exists", duration: 4000);

                return;
            }

            project.Name = model.ProjectName;
            await dbContext.SaveChangesAsync();
            _project = project;
            UpdatePageTitle();
            StateHasChanged();
        }
    }

    private async Task<Project?> LoadProject(MainDbContext dbContext)
    {
        return await dbContext.Projects.Include(p => p.Owner).Include(p => p.Environments).FirstOrDefaultAsync(p => p.Id == ProjectId);
    }

    private async Task OnContextButtonClick(RadzenSplitButtonItem? arg)
    {
        if (arg?.Value == null)
        {
            NavigationManager.NavigateTo("/projects");
            return;
        }

        switch (arg.Value)
        {
            case "delete":
                await DeleteProject();
                break;

            case "rename":
                await RenameProject();
                break;
        }
    }

    private async Task NewEnvironment()
    {
        var model = new EditProjectEnvironmentDialogModel
        {
            ProjectId = ProjectId,
            ProjectName = _project!.Name,
            AcceptButtonText = "Create",
        };
        await DialogService.OpenAsync<EditProjectEnvironmentDialog>("New Environment", new Dictionary<string, object>() { ["model"] = model });

        if (model.Accepted)
        {
            await using MainDbContext dbContext = await _dbContextFactory.CreateDbContextAsync();
            dbContext.ProjectEnvironments.Add(new ProjectEnvironment
            {
                ProjectId = ProjectId,
                Name = model.Name.Trim(),
            });

            try
            {
                await dbContext.SaveChangesAsync();
                _project = await LoadProject(dbContext);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, summary: "Unable to create environment", detail: ex.Message, duration: 4000);
            }
        }
    }
}