namespace Core.Domains.Users.Exception;

public sealed class InvalidNationalCodeException : DomainException
{
    public InvalidNationalCodeException() : base("NationalCode Format Is Invalid")
    {
    }
}