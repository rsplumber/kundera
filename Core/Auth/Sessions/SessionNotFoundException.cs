namespace Core.Auth.Sessions;

public sealed class SessionNotFoundException : CoreException
{
    private const int DefaultCode = 404;
    private const string DefaultMessage = "Session not found";

    public SessionNotFoundException() : base(DefaultCode, DefaultMessage)
    {
    }
}