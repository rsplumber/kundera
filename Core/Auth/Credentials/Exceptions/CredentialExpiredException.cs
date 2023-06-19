namespace Core.Auth.Credentials.Exceptions;

public sealed class CredentialExpiredException : CoreException
{
    private const int DefaultCode = 400;
    private const string DefaultMessage = "Credential expired";

    public CredentialExpiredException() : base(DefaultCode, DefaultMessage)
    {
    }
}