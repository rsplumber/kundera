namespace Core.Domains.Scopes.Exceptions;

public sealed class ScopeNotFoundException : NotSupportedException
{
    private const string DefaultMessage = "Scope not found";

    public ScopeNotFoundException() : base(DefaultMessage)
    {
    }
}