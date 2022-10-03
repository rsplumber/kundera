namespace Users.Domain;

public class DuplicateRoleNotAssignableException : NotSupportedException
{
    public DuplicateRoleNotAssignableException(string role) : base($" Role '{role}' already exists")
    {
    }
}