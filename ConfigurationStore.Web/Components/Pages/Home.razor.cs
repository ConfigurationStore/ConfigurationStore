using ConfigurationStore.Auth;
using ConfigurationStore.Data;

using Microsoft.EntityFrameworkCore;

namespace ConfigurationStore.Web.Components.Pages;

public partial class Home
{
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
        List<User> users = await dbContext.Users.ToListAsync();
        if (users.Any())
        {
            _authenticationState.SetLoggedIn(users[0]);
            return;
        }

        var user = new User
        {
            Username = "admin", DisplayName = "Administrator", PasswordHash = "==",
        };
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        _authenticationState.SetLoggedIn(user);
    }

    private void LogOutDummy()
    {
        _authenticationState.SetLoggedOut();
    }
}