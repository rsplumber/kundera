namespace Core.Domains.Auth.Credentials.Exceptions;

public sealed class WrongPasswordException : ApplicationException
{
    private const string DefaultMessage = "wrong password";

    public WrongPasswordException() : base(DefaultMessage)
    {
    }
}