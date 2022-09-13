using Tes.Domain.Contracts;
using Users.Domain.Types;

namespace Users.Domain.Exception;

public class InvalidEmailException : DomainException
{
    public InvalidEmailException(Email email) : base("Email Format Is Invalid")
    {
    }
}