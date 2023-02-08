namespace Core.Domains.Roles.Exceptions;

public sealed class RoleNotFoundException : KunderaException
{
    private const int DefaultCode = 404;
    private const string DefaultMessage = "Role not found";

    public RoleNotFoundException() : base(DefaultCode, DefaultMessage)
    {
    }
}