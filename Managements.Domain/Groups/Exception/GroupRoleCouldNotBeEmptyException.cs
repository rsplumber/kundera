using Managements.Domain.Contracts;

namespace Managements.Domain.Groups.Exception;

public sealed class GroupRoleCouldNotBeEmptyException : DomainException
{
    private const string DefaultMessage = "Group's roles could not be empty";

    public GroupRoleCouldNotBeEmptyException() : base(DefaultMessage)
    {
    }
}