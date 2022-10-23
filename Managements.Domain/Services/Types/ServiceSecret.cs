namespace Managements.Domain.Services.Types;

public sealed record ServiceSecret
{
    private readonly string _value;

    private ServiceSecret(string value)
    {
        _value = value;
    }

    public static ServiceSecret From(string value) => new(value);

    public static implicit operator string(ServiceSecret secret) => secret.Value;

    public static implicit operator ServiceSecret(string secret) => From(secret);

    public string Value => _value;

    public bool Equals(ServiceSecret? other)
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