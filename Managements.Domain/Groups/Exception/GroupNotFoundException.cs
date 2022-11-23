using Managements.Domain.Contracts;

namespace Managements.Domain.Groups.Exception;

public sealed class GroupNotFoundException : DomainException
{
    private const string DefaultMessage = "Group not found";

    public GroupNotFoundException() : base(DefaultMessage)
    {
    }
}