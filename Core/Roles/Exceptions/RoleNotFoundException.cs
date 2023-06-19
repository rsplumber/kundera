namespace Core.Roles.Exceptions;

public sealed class RoleNotFoundException : CoreException
{
    private const int DefaultCode = 404;
    private const string DefaultMessage = "Role not found";

    public RoleNotFoundException() : base(DefaultCode, DefaultMessage)
    {
    }
}