using Kite.Domain.Contracts;

namespace Auth.Core.Exceptions;

public class CredentialNotFoundException : DomainException
{
    private const string DefaultMessage = "Credential not found";

    public CredentialNotFoundException() : base(DefaultMessage)
    {
    }
}