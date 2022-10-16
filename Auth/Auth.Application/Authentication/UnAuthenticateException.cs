namespace Auth.Application.Authentication;

public class UnAuthenticateException : ApplicationException
{
    private const string DefaultMessage = "UnAuthenticate";

    public UnAuthenticateException() : base(DefaultMessage)
    {
    }
}