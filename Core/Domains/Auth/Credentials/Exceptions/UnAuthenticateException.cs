namespace Core.Domains.Auth.Credentials.Exceptions;

public sealed class UnAuthenticateException : ApplicationException
{
    private const string DefaultMessage = "UnAuthenticate";

    public UnAuthenticateException() : base(DefaultMessage)
    {
    }
}