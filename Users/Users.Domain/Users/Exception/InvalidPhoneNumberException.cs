using Tes.Domain.Contracts;

namespace Users.Domain.Users.Exception;

public class InvalidPhoneNumberException : DomainException
{
    public InvalidPhoneNumberException() : base("PhoneNumber Format Is Invalid")
    {
    }
}