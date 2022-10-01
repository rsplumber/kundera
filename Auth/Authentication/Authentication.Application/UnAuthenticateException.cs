using Tes.Domain.Contracts;

namespace Authentication.Application;

public class UnAuthenticateException : DomainException
{
    private const string DefaultMessage = "UnAuthenticate";

    public UnAuthenticateException() : base(DefaultMessage)
    {
    }
}