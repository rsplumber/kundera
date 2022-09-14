using Tes.Domain.Contracts;

namespace Users.Domain.Users.Exception;

public class UserDuplicateIdentifierException : DomainException
{
    public UserDuplicateIdentifierException(string identifier) : base($"User {identifier} exists")
    {
    }
}