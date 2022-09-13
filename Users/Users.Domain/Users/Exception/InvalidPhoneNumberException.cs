using Tes.Domain.Contracts;
using Users.Domain.Users.Types;

namespace Users.Domain.Users.Exception;

public class InvalidPhoneNumberException : DomainException
{
    public InvalidPhoneNumberException(PhoneNumber phoneNumber) : base("PhoneNumber Format Is Invalid")
    {
    }
}