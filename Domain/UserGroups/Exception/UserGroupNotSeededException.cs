using Tes.Domain.Contracts;

namespace Domain.UserGroups.Exception;

public class UserGroupNotSeededException : DomainException
{
    private const string DefaultMessage = "administrator usergroup not seeded";

    public UserGroupNotSeededException() : base(DefaultMessage)
    {
    }
}