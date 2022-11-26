namespace Core.Domains.Services.Exceptions;

public sealed class ServiceAlreadyExistsException : NotSupportedException
{
    public ServiceAlreadyExistsException(string service) : base($"{service} already exists")
    {
    }
}