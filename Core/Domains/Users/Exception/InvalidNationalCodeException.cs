namespace Core.Domains.Users.Exception;

public sealed class InvalidNationalCodeException : ApplicationException
{
    public InvalidNationalCodeException() : base("NationalCode Format Is Invalid")
    {
    }
}