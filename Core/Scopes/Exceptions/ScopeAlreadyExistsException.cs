namespace Core.Scopes.Exceptions;

public sealed class ScopeAlreadyExistsException : CoreException
{
    private const int DefaultCode = 400;

    public ScopeAlreadyExistsException(string scope) : base(DefaultCode, $"{scope} already exists")
    {
    }
}