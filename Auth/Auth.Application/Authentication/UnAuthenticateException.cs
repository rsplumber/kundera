using Kite.Domain.Contracts;

namespace Auth.Application.Authentication;

public class UnAuthenticateException : DomainException
{
    private const string DefaultMessage = "UnAuthenticate";

    public UnAuthenticateException() : base(DefaultMessage)
    {
    }
}