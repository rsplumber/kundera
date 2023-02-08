namespace Core.Domains.Groups.Exception;

public sealed class GroupNotFoundException : KunderaException
{
    private const int DefaultCode = 404;
    private const string DefaultMessage = "Group not found";

    public GroupNotFoundException() : base(DefaultCode, DefaultMessage)
    {
    }
}