namespace Core.Domains.Auth.Credentials.Exceptions;

public sealed class DuplicateUniqueIdentifierException : DomainException
{
    public DuplicateUniqueIdentifierException(string identifier) : base($"{identifier} already exists")
    {
    }
}