namespace Auth.Core.Exceptions;

public class UnAuthenticateException : ApplicationException
{
    private const string DefaultMessage = "UnAuthenticate";

    public UnAuthenticateException() : base(DefaultMessage)
    {
    }
}