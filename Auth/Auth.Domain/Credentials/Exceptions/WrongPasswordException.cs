using Tes.Domain.Contracts;

namespace Auth.Domain.Credentials.Exceptions;

public class WrongPasswordException : DomainException
{
    private const string DefaultMessage = "wrong password";

    public WrongPasswordException() : base(DefaultMessage)
    {
    }
}