using Tes.Domain.Contracts;

namespace Users.Domain.Users.Exception;

public class UserNotFoundException : DomainException
{
    private const string DefaultMessage = "User not found";

    public UserNotFoundException() : base(DefaultMessage)
    {
    }
}