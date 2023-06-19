namespace Core.Roles.Exceptions;

public sealed class RoleAlreadyExistsException : CoreException
{
    private const int DefaultCode = 400;

    public RoleAlreadyExistsException(string role) : base(DefaultCode, $"{role} already exists")
    {
    }
}