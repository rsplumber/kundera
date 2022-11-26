namespace Auth.Core.Exceptions;

public sealed class UnAuthenticateException : ApplicationException
{
    private const string DefaultMessage = "UnAuthenticate";

    public UnAuthenticateException() : base(DefaultMessage)
    {
    }
}