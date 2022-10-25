using Managements.Domain.Users.Exception;

namespace Managements.Domain.Groups.Types;

public sealed class GroupStatus
{
    public static readonly GroupStatus Enable = From(nameof(Enable));
    public static readonly GroupStatus Disable = From(nameof(Disable));

    private readonly string _value;

    private GroupStatus(string value)
    {
        _value = value;
        Validate();
    }

    public static GroupStatus From(string value) => new(value);

    private void Validate()
    {
        if (Value is not (nameof(Enable) or nameof(Disable)))
        {
            throw new UserStatusNotSupportedException(Value);
        }
    }

    public string Value => _value;

    private bool Equals(GroupStatus other)
    {
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is GroupStatus other && Equals(other);
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