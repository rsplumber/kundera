namespace Core.Domains.Auth.Credentials.Exceptions;

public sealed class InvalidScopeException : KunderaException
{
    private const int DefaultCode = 400;
    private const string DefaultMessage = "Invalid scope";

    public InvalidScopeException() : base(DefaultCode, DefaultMessage)
    {
    }
}