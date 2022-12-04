namespace Core.Domains.Users.Exception;

public sealed class UserDuplicateIdentifierException : DomainException
{
    public UserDuplicateIdentifierException(string identifier) : base($"User {identifier} exists")
    {
    }
}