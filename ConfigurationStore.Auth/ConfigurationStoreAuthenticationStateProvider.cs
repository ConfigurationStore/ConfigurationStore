using System.Security.Claims;

using ConfigurationStore.Data;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationStore.Auth;

public class ConfigurationStoreAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
    private static readonly ClaimsPrincipal _anonymousUser = new(new ClaimsIdentity());

    private ClaimsPrincipal _currentUser = _anonymousUser;

    public ConfigurationStoreAuthenticationStateProvider(IDbContextFactory<MainDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    public User? AuthenticatedUser { get; private set; }

    public override Task<AuthenticationState> GetAuthenticationStateAsync() => Task.FromResult(new AuthenticationState(_currentUser));

    public void SetLoggedIn(User user)
    {
        AuthenticatedUser = user;

        var claims = new List<Claim>()
        {
            new(ClaimTypes.Sid, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Username),
            new(ClaimTypes.Name, user.DisplayName),
        };

        var identity = new ClaimsIdentity(claims, authenticationType: "Custom");
        _currentUser = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public void SetLoggedOut()
    {
        AuthenticatedUser = null;
        _currentUser = _anonymousUser;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}