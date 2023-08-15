namespace Data.Auth.Credentials;

internal sealed class PasswordType
{
    public string Value { get; set; } = default!;

    public string Salt { get; set; } = default!;
}