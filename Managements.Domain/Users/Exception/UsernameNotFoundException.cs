using Kite.Domain.Contracts;

namespace Managements.Domain.Users.Exception;

public class UsernameNotFoundException : DomainException
{
    private const string DefaultMessage = "Username not found";

    public UsernameNotFoundException() : base(DefaultMessage)
    {
    }
}