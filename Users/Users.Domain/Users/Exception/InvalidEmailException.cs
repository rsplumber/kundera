using Tes.Domain.Contracts;

namespace Users.Domain.Users.Exception;

public class InvalidEmailException : DomainException
{
    public InvalidEmailException() : base("Email Format Is Invalid")
    {
    }
}