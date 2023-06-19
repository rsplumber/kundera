namespace Core.Auth.Authorizations;

public class ForbiddenException : CoreException
{
    private const int DefaultCode = 403;
    private const string DefaultMessage = "Forbidden";

    public ForbiddenException() : base(DefaultCode, DefaultMessage)
    {
    }
}