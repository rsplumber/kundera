using Tes.Domain.Contracts;

namespace Auth.Domain.Credentials.Exceptions;

public class DuplicateUniqueIdentifierException : DomainException
{
    public DuplicateUniqueIdentifierException(string identifier) : base($"{identifier} already exists")
    {
    }
}