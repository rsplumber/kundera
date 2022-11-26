using Managements.Domain.Contracts;

namespace Auth.Core.Exceptions;

public sealed class CredentialNotFoundException : DomainException
{
    private const string DefaultMessage = "Credential not found";

    public CredentialNotFoundException() : base(DefaultMessage)
    {
    }
}