namespace Core.Domains.Users.Exception;

public sealed class UserNotFoundException : KeyNotFoundException
{
    private const string DefaultMessage = "User not found";

    public UserNotFoundException() : base(DefaultMessage)
    {
    }
}