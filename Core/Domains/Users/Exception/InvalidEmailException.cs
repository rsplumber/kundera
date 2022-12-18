namespace Core.Domains.Users.Exception;

public sealed class InvalidEmailException : ApplicationException
{
    public InvalidEmailException() : base("Email Format Is Invalid")
    {
    }
}