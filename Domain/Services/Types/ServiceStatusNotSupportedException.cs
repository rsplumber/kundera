namespace Domain.Services.Types;

public class ServiceStatusNotSupportedException : NotSupportedException
{
    public ServiceStatusNotSupportedException(string status) : base($"{status} not supported")
    {
    }
}