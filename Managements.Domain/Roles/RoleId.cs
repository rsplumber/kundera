﻿using Kite.Domain.Contracts;

namespace Managements.Domain.Roles;

public sealed record RoleId : IEntityIdentity
{
    private readonly string _value;

    private RoleId(string value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));

        _value = value.Replace(" ", "")
            .ToLower();
    }

    public static RoleId From(string value) => new(value);

    public string Value => _value;

    public bool Equals(RoleId? other)
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