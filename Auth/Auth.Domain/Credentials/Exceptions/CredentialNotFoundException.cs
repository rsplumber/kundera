using Tes.Domain.Contracts;

namespace Auth.Domain.Credentials.Exceptions;

public class CredentialNotFoundException : DomainException
{
    private const string DefaultMessage = "Credential not found";

    public CredentialNotFoundException() : base(DefaultMessage)
    {
    }
}