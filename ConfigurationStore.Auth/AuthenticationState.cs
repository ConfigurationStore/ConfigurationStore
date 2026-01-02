using ConfigurationStore.Data;

namespace ConfigurationStore.Auth;

internal class AuthenticationState : IAuthenticationState
{
    public User? AuthenticatedUser { get; set; }
}