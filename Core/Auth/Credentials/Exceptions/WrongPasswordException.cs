namespace Core.Auth.Credentials.Exceptions;

public sealed class WrongPasswordException : CoreException
{
    private const int DefaultCode = 400;
    private const string DefaultMessage = "Wrong password";

    public WrongPasswordException() : base(DefaultCode, DefaultMessage)
    {
    }
}