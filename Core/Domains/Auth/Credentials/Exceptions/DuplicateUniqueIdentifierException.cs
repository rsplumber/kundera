namespace Core.Domains.Auth.Credentials.Exceptions;

public sealed class DuplicateUniqueIdentifierException : KunderaException
{
    private const int DefaultCode = 400;

    public DuplicateUniqueIdentifierException(string identifier) : base(DefaultCode, $"{identifier} already exists")
    {
    }
}