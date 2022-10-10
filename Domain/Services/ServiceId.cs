using Tes.Domain.Contracts;

namespace Domain.Services;

public sealed record ServiceId : IIdentity
{
    private readonly string _value;

    private ServiceId(string value)
    {
        _value = value.Replace(" ", "").ToLower();
    }

    public static ServiceId From(string value) => new(value);

    public string Value => _value;

    public bool Equals(ServiceId? other)
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