using Tes.Domain.Contracts;

namespace Authentication.Domain.Types;

public class Password : CustomType<string, Password>
{
    public static implicit operator string(Password password) => password.Value;

    public static implicit operator Password(string password) => From(password);
}