using Tes.Domain.Contracts;

namespace Authentication.Domain.Exceptions;

public class CredentialNotFoundException : DomainException
{
    private const string DefaultMessage = "Credential not found";

    public CredentialNotFoundException() : base(DefaultMessage)
    {
    }
}