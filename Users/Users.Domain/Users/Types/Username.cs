using Tes.Domain.Contracts;

namespace Users.Domain.Users.Types;

public class Username : CustomType<string, Username>
{
    public static implicit operator string(Username name) => name.Value;

    public static implicit operator Username(string name) => From(name);
}