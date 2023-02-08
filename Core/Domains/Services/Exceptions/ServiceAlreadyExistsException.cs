namespace Core.Domains.Services.Exceptions;

public sealed class ServiceAlreadyExistsException : KunderaException
{
    private const int DefaultCode = 400;

    public ServiceAlreadyExistsException(string service) : base(DefaultCode, $"{service} already exists")
    {
    }
}