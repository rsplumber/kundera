namespace Core.Domains.Users.Exception;

public sealed class UsernameNotFoundException : KeyNotFoundException
{
    private const string DefaultMessage = "Username not found";

    public UsernameNotFoundException() : base(DefaultMessage)
    {
    }
}