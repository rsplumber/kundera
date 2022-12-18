namespace Core.Domains.Groups.Exception;

public sealed class GroupNotFoundException : KeyNotFoundException
{
    private const string DefaultMessage = "Group not found";

    public GroupNotFoundException() : base(DefaultMessage)
    {
    }
}