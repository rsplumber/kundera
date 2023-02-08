namespace Core.Services;

public class ForbiddenException : KunderaException
{
    private const int DefaultCode = 403;
    private const string DefaultMessage = "Forbidden";

    public ForbiddenException() : base(DefaultCode, DefaultMessage)
    {
    }
}