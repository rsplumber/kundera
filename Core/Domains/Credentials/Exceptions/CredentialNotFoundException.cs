using Core.Domains.Contracts;

namespace Core.Domains.Credentials.Exceptions;

public sealed class CredentialNotFoundException : DomainException
{
    private const string DefaultMessage = "Credential not found";

    public CredentialNotFoundException() : base(DefaultMessage)
    {
    }
}