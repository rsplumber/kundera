namespace Core.Domains.Users.Exception;

public sealed class UserDuplicateIdentifierException : ApplicationException
{
    public UserDuplicateIdentifierException(string identifier) : base($"User {identifier} exists")
    {
    }
}