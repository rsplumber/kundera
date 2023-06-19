namespace Core.Auth.Credentials.Exceptions;

public sealed class WrongUsernamePasswordException : CoreException
{
    private const int DefaultCode = 400;
    private const string DefaultMessage = "Wrong username/password";

    public WrongUsernamePasswordException() : base(DefaultCode, DefaultMessage)
    {
    }
}