using Kite.Domain.Contracts;

namespace Auth.Core.Exceptions;

public class WrongPasswordException : DomainException
{
    private const string DefaultMessage = "wrong password";

    public WrongPasswordException() : base(DefaultMessage)
    {
    }
}