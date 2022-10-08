namespace Domain.Scopes.Exceptions;

public class ScopeAlreadyExistsException : NotSupportedException
{
    public ScopeAlreadyExistsException(string scope) : base($"{scope} already exists")
    {
    }
}