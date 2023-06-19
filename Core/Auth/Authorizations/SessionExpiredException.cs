namespace Core.Auth.Authorizations;

public class SessionExpiredException : CoreException
{
    private const int DefaultCode = 440;
    private const string DefaultMessage = "Session expired";

    public SessionExpiredException() : base(DefaultCode, DefaultMessage)
    {
    }
}