using ConfigurationStore.Data;

namespace ConfigurationStore.Auth;

public interface IUserManager
{
    Task<User> CreateUserAsync(string username, string displayName, string password, CancellationToken cancellationToken = default);
    Task<User?> TryLoginUserAsync(string username, string password, CancellationToken cancellationToken = default);
    Task<User> ChangePasswordAsync(string username, string oldPassword, string newPassword, CancellationToken cancellationToken = default);
}