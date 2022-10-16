using Kite.Domain.Contracts;

namespace Domain.UserGroups.Exception;

public class UserGroupNameDuplicateException : DomainException
{
    private const string DefaultMessage = "UserGroup name is duplicate";

    public UserGroupNameDuplicateException() : base(DefaultMessage)
    {
    }
}