using Tes.Domain.Contracts;

namespace Domain.Users.Exception;

public class InvalidPhoneNumberException : DomainException
{
    public InvalidPhoneNumberException() : base("PhoneNumber Format Is Invalid")
    {
    }
}