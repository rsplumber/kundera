using Core.Domains.Contracts;

namespace Core.Domains.Users.Exception;

public sealed class InvalidPhoneNumberException : DomainException
{
    public InvalidPhoneNumberException() : base("PhoneNumber Format Is Invalid")
    {
    }
}