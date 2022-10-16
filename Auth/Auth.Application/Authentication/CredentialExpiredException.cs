namespace Auth.Application.Authentication;

public class CredentialExpiredException : ApplicationException
{
    private const string DefaultMessage = "Credential expired";

    public CredentialExpiredException() : base(DefaultMessage)
    {
    }
}