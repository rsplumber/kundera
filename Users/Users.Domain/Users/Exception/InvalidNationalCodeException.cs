using Tes.Domain.Contracts;
using Users.Domain.Users.Types;

namespace Users.Domain.Users.Exception;

public class InvalidNationalCodeException : DomainException
{
    public InvalidNationalCodeException() : base("NationalCode Format Is Invalid")
    {
    }
}