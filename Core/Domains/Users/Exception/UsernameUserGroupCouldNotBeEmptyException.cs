using Core.Domains.Contracts;

namespace Core.Domains.Users.Exception;

public sealed class UsernameGroupCouldNotBeEmptyException : DomainException
{
    private const string DefaultMessage = "Username's group could not be empty";

    public UsernameGroupCouldNotBeEmptyException() : base(DefaultMessage)
    {
    }
}