namespace Core.Domains.Permissions.Exceptions;

public sealed class PermissionNotFoundException : KunderaException
{
    private const int DefaultCode = 404;
    private const string DefaultMessage = "Permission not found";

    public PermissionNotFoundException() : base(DefaultCode, DefaultMessage)
    {
    }
}