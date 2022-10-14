using Kite.Domain.Contracts;

namespace Domain.UserGroups.Exception;

public class UserGroupRoleCouldNotBeEmptyException : DomainException
{
    private const string DefaultMessage = "UserGroup's roles could not be empty";

    public UserGroupRoleCouldNotBeEmptyException() : base(DefaultMessage)
    {
    }
}