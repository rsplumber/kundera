using Core.Domains.Contracts;

namespace Core.Domains.Users.Exception;

public sealed class UserNotFoundException : DomainException
{
    private const string DefaultMessage = "User not found";

    public UserNotFoundException() : base(DefaultMessage)
    {
    }
}