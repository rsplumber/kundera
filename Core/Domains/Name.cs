namespace Core.Domains;

public sealed record Name
{
    private readonly string _value;

    private Name(string value)
    {
        _value = value.ToLower();
    }

    public static Name From(string value) => new(value);

    public static implicit operator string(Name name) => name.Value;

    public static implicit operator Name(string name) => From(name);

    public string Value => _value;

    public bool Equals(Name? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _value == other._value;
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