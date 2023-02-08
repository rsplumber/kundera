namespace Core.Domains.Services.Exceptions;

public sealed class ServiceNotFoundException : KunderaException
{
    private const int DefaultCode = 404;
    private const string DefaultMessage = "Service not found";

    public ServiceNotFoundException() : base(DefaultCode, DefaultMessage)
    {
    }
}