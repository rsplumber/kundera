using Kite.Domain.Contracts;

namespace Auth.Application.Authentication;

public class CredentialExpiredException : DomainException
{
    private const string DefaultMessage = "Credential expired";

    public CredentialExpiredException() : base(DefaultMessage)
    {
    }
}