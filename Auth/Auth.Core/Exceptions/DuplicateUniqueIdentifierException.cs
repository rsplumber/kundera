using Kite.Domain.Contracts;

namespace Auth.Core.Exceptions;

public class DuplicateUniqueIdentifierException : DomainException
{
    public DuplicateUniqueIdentifierException(string identifier) : base($"{identifier} already exists")
    {
    }
}