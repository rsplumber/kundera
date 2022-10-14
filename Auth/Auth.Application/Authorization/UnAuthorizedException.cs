using Kite.Domain.Contracts;

namespace Auth.Application.Authorization;

public class UnAuthorizedException : DomainException
{
    private const string DefaultMessage = "UnAuthorized";

    public UnAuthorizedException() : base(DefaultMessage)
    {
    }
}