using System.Text.RegularExpressions;
using Tes.Domain.Contracts;
using Users.Domain.Users.Exception;

namespace Users.Domain.Users.Types;

public class Email : CustomType<string, Email>
{
    private const string RegexPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

    public static implicit operator string(Email email) => email.Value;

    public static implicit operator Email(string email) => From(email);

    protected override void Validate()
    {
        if (!Regex.IsMatch(Value, RegexPattern, RegexOptions.IgnoreCase))
        {
            throw new InvalidEmailException(Value);
        }

        base.Validate();
    }
}