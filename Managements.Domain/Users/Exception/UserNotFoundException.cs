using Managements.Domain.Contracts;

namespace Managements.Domain.Users.Exception;

public sealed class UserNotFoundException : DomainException
{
    private const string DefaultMessage = "User not found";

    public UserNotFoundException() : base(DefaultMessage)
    {
    }
}