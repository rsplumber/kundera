namespace Domain.Users.Exception;

public class UserStatusNotSupportedException : NotSupportedException
{
    public UserStatusNotSupportedException(string status) : base($"{status} not supported")
    {
    }
}