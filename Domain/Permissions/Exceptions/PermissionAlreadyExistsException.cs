namespace Domain.Permissions.Exceptions;

public class PermissionAlreadyExistsException : NotSupportedException
{
    public PermissionAlreadyExistsException(string permission) : base($"{permission} already exists")
    {
    }
}