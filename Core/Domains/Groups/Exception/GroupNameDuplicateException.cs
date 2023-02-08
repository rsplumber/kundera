namespace Core.Domains.Groups.Exception;

public sealed class GroupNameDuplicateException : KunderaException
{
    private const int DefaultCode = 400;
    private const string DefaultMessage = "Group name is duplicate";

    public GroupNameDuplicateException() : base(DefaultCode, DefaultMessage)
    {
    }
}