using Tes.Domain.Contracts;
using Users.Domain.Users.Types;

namespace Users.Domain.Users.Exception;

public class InvalidEmailException : DomainException
{
    public InvalidEmailException(Email email) : base("Email Format Is Invalid")
    {
    }
}