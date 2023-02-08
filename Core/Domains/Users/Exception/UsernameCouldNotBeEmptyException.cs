namespace Core.Domains.Users.Exception;

public sealed class UsernameCouldNotBeEmptyException : KunderaException
{
    private const int DefaultCode = 400;
    private const string DefaultMessage = "Username could not be empty";

    public UsernameCouldNotBeEmptyException() : base(DefaultCode, DefaultMessage)
    {
    }
}