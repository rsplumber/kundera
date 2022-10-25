using Kite.Domain.Contracts;

namespace Managements.Domain.Users.Exception;

public class UsernameGroupCouldNotBeEmptyException : DomainException
{
    private const string DefaultMessage = "Username's group could not be empty";

    public UsernameGroupCouldNotBeEmptyException() : base(DefaultMessage)
    {
    }
}