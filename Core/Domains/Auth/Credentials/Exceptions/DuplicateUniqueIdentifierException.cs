namespace Core.Domains.Auth.Credentials.Exceptions;

public sealed class DuplicateUniqueIdentifierException : ApplicationException
{
    public DuplicateUniqueIdentifierException(string identifier) : base($"{identifier} already exists")
    {
    }
}