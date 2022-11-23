using Managements.Domain.Contracts;

namespace Managements.Domain.Users.Exception;

public sealed class UsernameNotFoundException : DomainException
{
    private const string DefaultMessage = "Username not found";

    public UsernameNotFoundException() : base(DefaultMessage)
    {
    }
}