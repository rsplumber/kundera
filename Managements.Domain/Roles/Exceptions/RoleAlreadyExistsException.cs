namespace Managements.Domain.Roles.Exceptions;

public class RoleAlreadyExistsException : NotSupportedException
{
    public RoleAlreadyExistsException(string role) : base($"{role} already exists")
    {
    }
}