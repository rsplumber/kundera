namespace Core.Domains.Auth.Credentials.Exceptions;

public sealed class CredentialNotFoundException : KunderaException
{
    private const int DefaultCode = 404;
    private const string DefaultMessage = "Credential not found";

    public CredentialNotFoundException() : base(DefaultCode, DefaultMessage)
    {
    }
}