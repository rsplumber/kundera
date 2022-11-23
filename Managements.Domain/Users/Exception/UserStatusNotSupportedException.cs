namespace Managements.Domain.Users.Exception;

public sealed class UserStatusNotSupportedException : NotSupportedException
{
    public UserStatusNotSupportedException(string status) : base($"{status} not supported")
    {
    }
}