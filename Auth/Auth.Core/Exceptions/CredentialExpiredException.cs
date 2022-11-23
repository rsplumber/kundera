namespace Auth.Core.Exceptions;

public sealed class CredentialExpiredException : ApplicationException
{
    private const string DefaultMessage = "Credential expired";

    public CredentialExpiredException() : base(DefaultMessage)
    {
    }
}