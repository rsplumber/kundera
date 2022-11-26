using Core.Domains.Contracts;

namespace Core.Domains.Users.Exception;

public sealed class InvalidEmailException : DomainException
{
    public InvalidEmailException() : base("Email Format Is Invalid")
    {
    }
}