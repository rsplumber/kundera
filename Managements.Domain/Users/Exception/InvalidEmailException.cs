using Managements.Domain.Contracts;

namespace Managements.Domain.Users.Exception;

public sealed class InvalidEmailException : DomainException
{
    public InvalidEmailException() : base("Email Format Is Invalid")
    {
    }
}