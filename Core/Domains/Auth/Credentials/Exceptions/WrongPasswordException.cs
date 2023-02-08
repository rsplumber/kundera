namespace Core.Domains.Auth.Credentials.Exceptions;

public sealed class WrongPasswordException : KunderaException
{
    private const int DefaultCode = 400;
    private const string DefaultMessage = "Wrong password";

    public WrongPasswordException() : base(DefaultCode, DefaultMessage)
    {
    }
}