namespace Core.Domains.Permissions.Exceptions;

public sealed class PermissionAlreadyExistsException : KunderaException
{
    private const int DefaultCode = 400;

    public PermissionAlreadyExistsException(string permission) : base(DefaultCode, $"{permission} already exists")
    {
    }
}