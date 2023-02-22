namespace Core.Domains.Auth.Sessions;

public sealed class SessionNotFoundException : KunderaException
{
    private const int DefaultCode = 404;
    private const string DefaultMessage = "Session not found";

    public SessionNotFoundException() : base(DefaultCode, DefaultMessage)
    {
    }
}