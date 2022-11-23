namespace Managements.Domain.Permissions.Exceptions;

public sealed class PermissionNotFoundException : NotSupportedException
{
    private const string DefaultMessage = "Permission not found";

    public PermissionNotFoundException() : base(DefaultMessage)
    {
    }
}