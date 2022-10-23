namespace Managements.Domain.Services.Types;

public sealed record ServiceStatus
{
    public static readonly ServiceStatus Active = From(nameof(Active));
    public static readonly ServiceStatus DeActive = From(nameof(DeActive));

    private readonly string _value;

    private ServiceStatus(string value)
    {
        _value = value;
        Validate();
    }

    public static ServiceStatus From(string value) => new(value);

    private void Validate()
    {
        if (Value is not (nameof(Active) or nameof(DeActive)))
        {
            throw new ServiceStatusNotSupportedException(Value);
        }
    }

    public string Value => _value;

    public bool Equals(ServiceStatus? other)
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