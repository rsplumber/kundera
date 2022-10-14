using Kite.Domain.Contracts;

namespace Domain.UserGroups.Exception;

public class UserGroupNotFoundException : DomainException
{
    private const string DefaultMessage = "UserGroup not found";

    public UserGroupNotFoundException() : base(DefaultMessage)
    {
    }
}