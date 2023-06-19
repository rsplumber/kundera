namespace Core.Services.Exceptions;

public sealed class ServiceNotFoundException : CoreException
{
    private const int DefaultCode = 404;
    private const string DefaultMessage = "Service not found";

    public ServiceNotFoundException() : base(DefaultCode, DefaultMessage)
    {
    }
}