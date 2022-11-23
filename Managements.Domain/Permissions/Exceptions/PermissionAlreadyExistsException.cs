namespace Managements.Domain.Permissions.Exceptions;

public sealed class PermissionAlreadyExistsException : NotSupportedException
{
    public PermissionAlreadyExistsException(string permission) : base($"{permission} already exists")
    {
    }
}