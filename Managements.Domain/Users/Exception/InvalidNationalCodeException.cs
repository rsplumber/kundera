using Kite.Domain.Contracts;

namespace Managements.Domain.Users.Exception;

public class InvalidNationalCodeException : DomainException
{
    public InvalidNationalCodeException() : base("NationalCode Format Is Invalid")
    {
    }
}