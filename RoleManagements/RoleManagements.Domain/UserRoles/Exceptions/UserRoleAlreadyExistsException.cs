namespace RoleManagements.Domain.UserRoles.Exceptions;

public class UserRoleAlreadyExistsException : NotSupportedException
{
    public UserRoleAlreadyExistsException(UserId user) : base($"user: {user} already exists")
    {
    }
}