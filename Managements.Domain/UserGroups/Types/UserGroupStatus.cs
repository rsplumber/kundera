using Managements.Domain.Users.Exception;

namespace Managements.Domain.UserGroups.Types;

public sealed class UserGroupStatus
{
    public static readonly UserGroupStatus Enable = From(nameof(Enable));
    public static readonly UserGroupStatus Disable = From(nameof(Disable));

    private readonly string _value;

    private UserGroupStatus(string value)
    {
        _value = value;
        Validate();
    }

    public static UserGroupStatus From(string value) => new(value);

    private void Validate()
    {
        if (Value is not (nameof(Enable) or nameof(Disable)))
        {
            throw new UserStatusNotSupportedException(Value);
        }
    }

    public string Value => _value;

    private bool Equals(UserGroupStatus other)
    {
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is UserGroupStatus other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public override string ToString()
    {
        return _value;
    }
}