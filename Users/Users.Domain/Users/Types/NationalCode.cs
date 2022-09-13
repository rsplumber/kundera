using System.Text.RegularExpressions;
using Tes.Domain.Contracts;
using Users.Domain.Users.Exception;

namespace Users.Domain.Users.Types;

public class NationalCode : CustomType<string, NationalCode>
{
    private const string RegexPattern = @"^(?!(\d)\1{9})\d{10}$";

    public static implicit operator string(NationalCode nationalCode) => nationalCode.Value;

    public static implicit operator NationalCode(string nationalCode) => From(nationalCode);

    protected override void Validate()
    {
        if (!Regex.IsMatch(Value, RegexPattern, RegexOptions.IgnoreCase))
            throw new InvalidNationalCodeException(Value);

        var check = Convert.ToInt32(Value.Substring(9, 1));
        var sum = Enumerable.Range(0, 9)
            .Select(x => Convert.ToInt32(Value.Substring(x, 1)) * (10 - x))
            .Sum() % 11;

        if (!(sum < 2 ? check == sum : check + sum == 11))
        {
            throw new InvalidNationalCodeException(Value);
        }

        base.Validate();
    }
}