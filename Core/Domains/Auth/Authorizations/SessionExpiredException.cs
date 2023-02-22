namespace Core.Domains.Auth.Authorizations;

public class SessionExpiredException : KunderaException
{
    private const int DefaultCode = 440;
    private const string DefaultMessage = "Session expired";

    public SessionExpiredException() : base(DefaultCode, DefaultMessage)
    {
    }
}