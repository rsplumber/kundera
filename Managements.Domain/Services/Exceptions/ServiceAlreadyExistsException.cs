namespace Managements.Domain.Services.Exceptions;

public class ServiceAlreadyExistsException : NotSupportedException
{
    public ServiceAlreadyExistsException(string service) : base($"{service} already exists")
    {
    }
}