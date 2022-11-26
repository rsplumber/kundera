namespace Core.Domains.Roles.Exceptions;

public sealed class RoleNotFoundException : NotSupportedException
{
    private const string DefaultMessage = "Role not found";

    public RoleNotFoundException() : base(DefaultMessage)
    {
    }
}