namespace Core.Domains.Groups.Exception;

public sealed class GroupRoleCouldNotBeEmptyException : KunderaException
{
    private const int DefaultCode = 400;
    private const string DefaultMessage = "Group's roles could not be empty";

    public GroupRoleCouldNotBeEmptyException() : base(DefaultCode, DefaultMessage)
    {
    }
}