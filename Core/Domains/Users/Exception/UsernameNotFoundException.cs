namespace Core.Domains.Users.Exception;

public sealed class UsernameNotFoundException : KunderaException
{
    private const int DefaultCode = 404;
    private const string DefaultMessage = "Username not found";

    public UsernameNotFoundException() : base(DefaultCode, DefaultMessage)
    {
    }
}