namespace Core.Domains.Users.Exception;

public sealed class UsernameGroupCouldNotBeEmptyException : KeyNotFoundException
{
    private const string DefaultMessage = "Username's group could not be empty";

    public UsernameGroupCouldNotBeEmptyException() : base(DefaultMessage)
    {
    }
}