namespace Core.Scopes.Exceptions;

public sealed class ScopeNotFoundException : CoreException
{
    private const int DefaultCode = 404;
    private const string DefaultMessage = "Scope not found";

    public ScopeNotFoundException() : base(DefaultCode, DefaultMessage)
    {
    }
}