using Core.Domains.Contracts;

namespace Core.Domains.Groups.Exception;

public sealed class GroupNotFoundException : DomainException
{
    private const string DefaultMessage = "Group not found";

    public GroupNotFoundException() : base(DefaultMessage)
    {
    }
}