using ConfigurationStore.Data;

namespace ConfigurationStore.Auth;

public interface IAuthenticationState
{
    User? AuthenticatedUser { get; set; }
}