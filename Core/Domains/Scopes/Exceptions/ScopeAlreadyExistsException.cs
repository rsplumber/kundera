namespace Core.Domains.Scopes.Exceptions;

public sealed class ScopeAlreadyExistsException : KunderaException
{
    private const int DefaultCode = 400;

    public ScopeAlreadyExistsException(string scope) : base(DefaultCode, $"{scope} already exists")
    {
    }
}