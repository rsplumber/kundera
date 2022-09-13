using Tes.Domain.Contracts;
using Users.Domain.Types;

namespace Users.Domain.Exception;

public class InvalidPhoneNumberException : DomainException
{
    public InvalidPhoneNumberException(PhoneNumber phoneNumber) : base("PhoneNumber Format Is Invalid")
    {
    }
}