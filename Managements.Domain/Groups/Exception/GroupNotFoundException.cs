using Kite.Domain.Contracts;

namespace Managements.Domain.Groups.Exception;

public class GroupNotFoundException : DomainException
{
    private const string DefaultMessage = "Group not found";

    public GroupNotFoundException() : base(DefaultMessage)
    {
    }
}