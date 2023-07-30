namespace Core.Auth.Credentials.Exceptions;

public sealed class RefreshTokenExpiredException : CoreException
{
    private const int DefaultCode = 450;
    private const string DefaultMessage = "RefreshToken expired, login required";

    public RefreshTokenExpiredException() : base(DefaultCode, DefaultMessage)
    {
    }
}