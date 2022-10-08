namespace Domain.Permissions.Exceptions;

public class PermissionNotFoundException : NotSupportedException
{
    private const string DefaultMessage = "Permission not found";
    public PermissionNotFoundException() : base(DefaultMessage)
    {
    }
}