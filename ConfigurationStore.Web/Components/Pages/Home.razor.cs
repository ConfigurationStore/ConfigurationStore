using ConfigurationStore.Auth;
using ConfigurationStore.Data;

using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationStore.Web.Components.Pages;

public partial class Home
{
    [CascadingParameter]
    public Action<string>? SetPageTitle { get; set; }

    private readonly ConfigurationStoreAuthenticationStateProvider _authenticationState;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly IDbContextFactory<MainDbContext> _dbContextFactory;

    public Home(ConfigurationStoreAuthenticationStateProvider authenticationState, IHostEnvironment hostEnvironment,
            IDbContextFactory<MainDbContext> dbContextFactory)
    {
        _authenticationState = authenticationState ?? throw new ArgumentNullException(nameof(authenticationState));
        _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    protected override async Task OnInitializedAsync()
    {
        SetPageTitle?.Invoke("Login");
        await base.OnInitializedAsync();
        await LogInIfLoggedOut();
    }

    private async Task LogInIfLoggedOut()
    {
        if (_authenticationState.AuthenticatedUser == null)
        {
            await LogInAdministratorInDevelopment();
        }
    }

    private async Task LogInAdministratorInDevelopment()
    {
        await using MainDbContext dbContext = await _dbContextFactory.CreateDbContextAsync();
        User? user = await dbContext.Users
           .Include(u => u.Groups)
           .FirstOrDefaultAsync(u => u.Username == "admin");
        if (user != null)
        {
            _authenticationState.SetLoggedIn(user);
        }
    }

    private async Task LogInDeveloperInDevelopment()
    {
        await using MainDbContext dbContext = await _dbContextFactory.CreateDbContextAsync();
        User? user = await dbContext.Users
           .Include(u => u.Groups)
           .FirstOrDefaultAsync(u => u.Username == "developer");
        if (user != null)
        {
            _authenticationState.SetLoggedIn(user);
        }
    }

    private void LogOut()
    {
        _authenticationState.SetLoggedOut();
    }
}