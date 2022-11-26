namespace Core.Domains.Permissions.Exceptions;

public sealed class PermissionAlreadyExistsException : NotSupportedException
{
    public PermissionAlreadyExistsException(string permission) : base($"{permission} already exists")
    {
    }
}