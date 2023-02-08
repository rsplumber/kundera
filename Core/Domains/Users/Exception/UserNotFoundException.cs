namespace Core.Domains.Users.Exception;

public sealed class UserNotFoundException : KunderaException
{
    private const int DefaultCode = 404;
    private const string DefaultMessage = "User not found";

    public UserNotFoundException() : base(DefaultCode,DefaultMessage)
    {
    }
}