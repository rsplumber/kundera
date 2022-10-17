using Kite.Domain.Contracts;

namespace Managements.Domain.Users.Exception;

public class InvalidPhoneNumberException : DomainException
{
    public InvalidPhoneNumberException() : base("PhoneNumber Format Is Invalid")
    {
    }
}