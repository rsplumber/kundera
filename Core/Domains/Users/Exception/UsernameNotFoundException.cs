namespace Core.Domains.Users.Exception;

public sealed class UsernameNotFoundException : DomainException
{
    private const string DefaultMessage = "Username not found";

    public UsernameNotFoundException() : base(DefaultMessage)
    {
    }
}