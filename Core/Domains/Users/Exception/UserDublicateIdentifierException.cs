namespace Core.Domains.Users.Exception;

public sealed class UserDuplicateIdentifierException : KunderaException
{
    private const int DefaultCode = 400;

    public UserDuplicateIdentifierException(string identifier) : base(DefaultCode, $"User {identifier} exists")
    {
    }
}