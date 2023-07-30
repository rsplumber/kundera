namespace Core.Auth.Credentials.Exceptions;

public sealed class InvalidCertificateException : CoreException
{
    private const int DefaultCode = 401;
    private const string DefaultMessage = "Unauthorized";

    public InvalidCertificateException() : base(DefaultCode, DefaultMessage)
    {
    }
}