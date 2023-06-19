namespace Core.Groups.Exception;

public sealed class GroupNameDuplicateException : CoreException
{
    private const int DefaultCode = 400;
    private const string DefaultMessage = "Group name is duplicate";

    public GroupNameDuplicateException() : base(DefaultCode, DefaultMessage)
    {
    }
}