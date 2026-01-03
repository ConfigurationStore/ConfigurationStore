using ConfigurationStore.Auth;
using ConfigurationStore.Data;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace ConfigurationStore.Web.Components.Pages;

public partial class Home
{
    protected override string PageTitle => "Login";

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
            CheckRedirect();
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
            CheckRedirect();
        }
    }

    private void CheckRedirect()
    {
        Dictionary<string, StringValues> query = QueryHelpers.ParseQuery(new Uri(NavigationManager.Uri).Query);
        if (query.TryGetValue("returnUrl", out StringValues returnUrl))
        {
            NavigationManager.NavigateTo(returnUrl[0]!);
        }
    }

    private void LogOut()
    {
        _authenticationState.SetLoggedOut();
    }
}