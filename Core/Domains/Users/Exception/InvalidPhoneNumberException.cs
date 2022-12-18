namespace Core.Domains.Users.Exception;

public sealed class InvalidPhoneNumberException : ApplicationException
{
    public InvalidPhoneNumberException() : base("PhoneNumber Format Is Invalid")
    {
    }
}