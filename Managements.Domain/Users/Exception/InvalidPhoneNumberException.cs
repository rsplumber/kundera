using Managements.Domain.Contracts;

namespace Managements.Domain.Users.Exception;

public sealed class InvalidPhoneNumberException : DomainException
{
    public InvalidPhoneNumberException() : base("PhoneNumber Format Is Invalid")
    {
    }
}