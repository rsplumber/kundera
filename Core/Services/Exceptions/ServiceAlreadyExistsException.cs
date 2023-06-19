namespace Core.Services.Exceptions;

public sealed class ServiceAlreadyExistsException : CoreException
{
    private const int DefaultCode = 400;

    public ServiceAlreadyExistsException(string service) : base(DefaultCode, $"{service} already exists")
    {
    }
}