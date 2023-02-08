namespace Core.Domains.Roles.Exceptions;

public sealed class RoleAlreadyExistsException : KunderaException
{
    private const int DefaultCode = 400;

    public RoleAlreadyExistsException(string role) : base(DefaultCode, $"{role} already exists")
    {
    }
}