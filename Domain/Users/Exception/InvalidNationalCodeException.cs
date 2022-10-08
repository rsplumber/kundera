using Tes.Domain.Contracts;

namespace Domain.Users.Exception;

public class InvalidNationalCodeException : DomainException
{
    public InvalidNationalCodeException() : base("NationalCode Format Is Invalid")
    {
    }
}