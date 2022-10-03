using Tes.Domain.Contracts;

namespace Authorization.Application;

public class UnAuthorizedException : DomainException
{
    private const string DefaultMessage = "UnAuthorized";

    public UnAuthorizedException() : base(DefaultMessage)
    {
    }
}