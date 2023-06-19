namespace Core.Permissions.Exceptions;

public sealed class PermissionAlreadyExistsException : CoreException
{
    private const int DefaultCode = 400;

    public PermissionAlreadyExistsException(string permission) : base(DefaultCode, $"{permission} already exists")
    {
    }
}