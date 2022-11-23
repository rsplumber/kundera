namespace Managements.Domain.Roles.Exceptions;

public sealed class RoleAlreadyExistsException : NotSupportedException
{
    public RoleAlreadyExistsException(string role) : base($"{role} already exists")
    {
    }
}