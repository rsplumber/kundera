using Tes.Domain.Contracts;

namespace Authentication.Domain.Types;

public class OneTimeToken : CustomType<string, OneTimeToken>
{
    public static implicit operator string(OneTimeToken oneTimeToken) => oneTimeToken.Value;
}