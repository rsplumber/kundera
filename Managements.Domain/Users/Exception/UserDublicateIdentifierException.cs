using Managements.Domain.Contracts;

namespace Managements.Domain.Users.Exception;

public sealed class UserDuplicateIdentifierException : DomainException
{
    public UserDuplicateIdentifierException(string identifier) : base($"User {identifier} exists")
    {
    }
}