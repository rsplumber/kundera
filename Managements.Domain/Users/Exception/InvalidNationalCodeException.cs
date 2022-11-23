using Managements.Domain.Contracts;

namespace Managements.Domain.Users.Exception;

public sealed class InvalidNationalCodeException : DomainException
{
    public InvalidNationalCodeException() : base("NationalCode Format Is Invalid")
    {
    }
}