namespace Core.Users.Exception;

public sealed class UsernameNotFoundException : CoreException
{
    private const int DefaultCode = 404;
    private const string DefaultMessage = "Username not found";

    public UsernameNotFoundException() : base(DefaultCode, DefaultMessage)
    {
    }
}