using System.Text.RegularExpressions;
using Tes.Domain.Contracts;
using Users.Domain.Users.Exception;

namespace Users.Domain.Users.Types;

public class PhoneNumber : CustomType<string, PhoneNumber>
{
    private const string RegexPattern = "09[0-3][0-9]-?[0-9]{3}-?[0-9]{4}";

    public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber.Value;

    public static implicit operator PhoneNumber(string phoneNumber) => From(phoneNumber);

    protected override void Validate()
    {
        if (!Regex.IsMatch(Value, RegexPattern, RegexOptions.IgnoreCase))
        {
            throw new InvalidPhoneNumberException();
        }

        base.Validate();
    }
}