namespace Core.Auth.Credentials.Exceptions;

public sealed class DuplicateUniqueIdentifierException : CoreException
{
    private const int DefaultCode = 400;

    public DuplicateUniqueIdentifierException(string identifier) : base(DefaultCode, $"{identifier} already exists")
    {
    }
}