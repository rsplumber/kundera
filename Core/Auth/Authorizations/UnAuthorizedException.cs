namespace Core.Auth.Authorizations;

public sealed class UnAuthorizedException : CoreException
{
    private const int DefaultCode = 401;
    private const string DefaultMessage = "Unauthorized";

    public UnAuthorizedException() : base(DefaultCode, DefaultMessage)
    {
    }
}