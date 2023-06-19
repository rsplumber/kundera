namespace Core.Users.Exception;

public sealed class UsernameCouldNotBeEmptyException : CoreException
{
    private const int DefaultCode = 400;
    private const string DefaultMessage = "Username could not be empty";

    public UsernameCouldNotBeEmptyException() : base(DefaultCode, DefaultMessage)
    {
    }
}