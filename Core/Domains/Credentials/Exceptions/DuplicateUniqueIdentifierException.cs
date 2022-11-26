using Core.Domains.Contracts;

namespace Core.Domains.Credentials.Exceptions;

public sealed class DuplicateUniqueIdentifierException : DomainException
{
    public DuplicateUniqueIdentifierException(string identifier) : base($"{identifier} already exists")
    {
    }
}