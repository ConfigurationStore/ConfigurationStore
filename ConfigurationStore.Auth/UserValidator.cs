using System.Text.RegularExpressions;

namespace ConfigurationStore.Auth;

public partial class UserValidator
{
    [GeneratedRegex(@"^[a-zA-Z0-9_\-]{5,}$")]
    private static partial Regex UsernamePattern();

    public static bool IsValidUsername(string username) => UsernamePattern().IsMatch(username);

    public static bool IsValidPassword(string password) => password.Length >= 12
                                                        && password.Any(char.IsUpper)
                                                        && password.Any(char.IsLower)
                                                        && password.Any(char.IsDigit)
                                                        && password.Any(char.IsSymbol);
}