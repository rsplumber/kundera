namespace RoleManagements.Domain.Services.Exceptions;

public class ServiceNotFoundException : NotSupportedException
{
    private const string DefaultMessage = "Service not found";
    public ServiceNotFoundException() : base(DefaultMessage)
    {
    }
}