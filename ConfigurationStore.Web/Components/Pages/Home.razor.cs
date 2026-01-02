using ConfigurationStore.Auth;
using ConfigurationStore.Data;

namespace ConfigurationStore.Web.Components.Pages;

public partial class Home
{
    private readonly ConfigurationStoreAuthenticationStateProvider _authenticationState;

    public Home(ConfigurationStoreAuthenticationStateProvider authenticationState)
    {
        _authenticationState = authenticationState ?? throw new ArgumentNullException(nameof(authenticationState));
    }

    private void LogInDummy()
    {
        if (_authenticationState.AuthenticatedUser != null)
        {
            _authenticationState.SetLoggedOut();
        }
        else
        {
            _authenticationState.SetLoggedIn(new User { Username = "dummy", DisplayName = "Dummy User", PasswordHash = "" });
        }
    }
}