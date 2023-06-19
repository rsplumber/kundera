namespace Core.Users.Exception;

public sealed class UsernameGroupCouldNotBeEmptyException : CoreException
{
    private const int DefaultCode = 400;
    private const string DefaultMessage = "Username's group could not be empty";

    public UsernameGroupCouldNotBeEmptyException() : base(DefaultCode, DefaultMessage)
    {
    }
}