using Managements.Domain.Contracts;

namespace Auth.Core.Exceptions;

public sealed class DuplicateUniqueIdentifierException : DomainException
{
    public DuplicateUniqueIdentifierException(string identifier) : base($"{identifier} already exists")
    {
    }
}