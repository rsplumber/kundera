namespace Managements.Domain.Scopes.Exceptions;

public sealed class ScopeAlreadyExistsException : NotSupportedException
{
    public ScopeAlreadyExistsException(string scope) : base($"{scope} already exists")
    {
    }
}