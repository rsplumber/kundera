using ValueOf;

namespace Managements.Domain.Users;

public sealed class Username : ValueOf<string, Username>
{
    public static implicit operator string(Username username) => username.Value;

    public static implicit operator Username(string name) => From(name);
}