namespace Core.Services;

public class SessionExpiredException : ApplicationException
{
    private const string DefaultMessage = "Session Expired";

    public SessionExpiredException() : base(DefaultMessage)
    {
    }
}