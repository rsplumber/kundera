using Kite.Domain.Contracts;

namespace Managements.Domain.Users.Exception;

public class UsernameCouldNotBeEmptyException : DomainException
{
    private const string DefaultMessage = "Username could not be empty";

    public UsernameCouldNotBeEmptyException() : base(DefaultMessage)
    {
    }
}