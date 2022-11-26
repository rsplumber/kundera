using Core.Domains.Contracts;

namespace Core.Domains.Credentials.Exceptions;

public sealed class WrongPasswordException : DomainException
{
    private const string DefaultMessage = "wrong password";

    public WrongPasswordException() : base(DefaultMessage)
    {
    }
}