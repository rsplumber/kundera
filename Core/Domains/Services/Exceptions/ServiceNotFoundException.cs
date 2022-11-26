namespace Core.Domains.Services.Exceptions;

public sealed class ServiceNotFoundException : NotSupportedException
{
    private const string DefaultMessage = "Service not found";

    public ServiceNotFoundException() : base(DefaultMessage)
    {
    }
}