namespace Core.Users.Exception;

public sealed class UserDuplicateIdentifierException : CoreException
{
    private const int DefaultCode = 400;

    public UserDuplicateIdentifierException(string identifier) : base(DefaultCode, $"User {identifier} exists")
    {
    }
}