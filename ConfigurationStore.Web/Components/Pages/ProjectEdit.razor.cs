using ConfigurationStore.Data;

using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

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

        Project? project = await dbContext.Projects.Include(p => p.Owner).FirstOrDefaultAsync(p => p.Id == ProjectId);
        _model = project != null ? new ProjectEditModel(project) : null;
        UpdatePageTitle();
    }
}