using Tes.Domain.Contracts;

namespace Users.Domain.Users.Types;

public class Text : CustomType<string, Text>
{
    public static implicit operator string?(Text? text) => text?.Value;

    public static implicit operator Text?(string? text)
    {
        return text != null ? From(text) : null;
    }
}