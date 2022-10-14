namespace Domain.Roles.Exceptions;

public class RoleNotFoundException : NotSupportedException
{
    private const string DefaultMessage = "Role not found";

    public RoleNotFoundException() : base(DefaultMessage)
    {
    }
}