namespace Core.Auth.Credentials.Exceptions;

public sealed class InvalidScopeException : CoreException
{
    private const int DefaultCode = 400;
    private const string DefaultMessage = "Invalid scope";

    public InvalidScopeException() : base(DefaultCode, DefaultMessage)
    {
    }
}