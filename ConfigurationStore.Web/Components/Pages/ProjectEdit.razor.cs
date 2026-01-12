using ConfigurationStore.Data;
using ConfigurationStore.Web.Components.Dialogs;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

using Radzen;
using Radzen.Blazor;

namespace ConfigurationStore.Web.Components.Pages;

public partial class ProjectEdit
{
    private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
    protected override string PageTitle  => _model == null ? "Unknown project" : _model.Name;

    private ProjectEditModel? _model;

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
        Project? project = await LoadProject(dbContext);
        _model = project != null ? new ProjectEditModel(project) : null;
        UpdatePageTitle();
    }

    private async Task DeleteProject()
    {
        bool? result = await DialogService.Confirm(
            message: ConfirmDeleteContent,
            title: "Really delete project " + _model!.Name, new ConfirmOptions
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
                    detail: $"Project {_model.Name} no longer exists", duration: 4000);

                return;
            }

            dbContext.Projects.Remove(project);
            await dbContext.SaveChangesAsync();

            NotificationService.Notify(NotificationSeverity.Success, summary: "Project " + _model.Name + " deleted successfully", duration: 4000);
            NavigationManager.NavigateTo("/projects");
        }
    }

    private async Task RenameProject()
    {
        var model = new RenameProjectDialogModel
        {
            ProjectName = _model!.Name,
        };
        await DialogService.OpenAsync<RenameProjectDialog>("Rename Project", new Dictionary<string, object>() { ["model"] = model });
        Console.WriteLine(model.Accepted + ": " + model.ProjectName);

        if (model.Accepted)
        {
            await using MainDbContext dbContext = await _dbContextFactory.CreateDbContextAsync();
            Project? project = await LoadProject(dbContext);
            if (project == null)
            {
                NotificationService.Notify(NotificationSeverity.Error, summary: "Project not found",
                        detail: $"Project {_model.Name} no longer exists", duration: 4000);

                return;
            }

            project.Name = model.ProjectName;
            await dbContext.SaveChangesAsync();
            _model = new ProjectEditModel(project);
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
}