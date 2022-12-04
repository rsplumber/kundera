namespace Core.Domains.Auth.Credentials.Exceptions;

public sealed class CredentialNotFoundException : DomainException
{
    private const string DefaultMessage = "Credential not found";

    public CredentialNotFoundException() : base(DefaultMessage)
    {
    }
}