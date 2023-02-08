namespace Core.Domains.Auth.Credentials.Exceptions;

public sealed class WrongUsernamePasswordException : KunderaException
{
    private const int DefaultCode = 400;
    private const string DefaultMessage = "Wrong username/password";

    public WrongUsernamePasswordException() : base(DefaultCode, DefaultMessage)
    {
    }
}