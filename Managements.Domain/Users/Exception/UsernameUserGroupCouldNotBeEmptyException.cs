using Kite.Domain.Contracts;

namespace Managements.Domain.Users.Exception;

public class UsernameUserGroupCouldNotBeEmptyException : DomainException
{
    private const string DefaultMessage = "Username's usergroup could not be empty";

    public UsernameUserGroupCouldNotBeEmptyException() : base(DefaultMessage)
    {
    }
}