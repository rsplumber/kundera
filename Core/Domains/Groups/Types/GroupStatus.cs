﻿using Core.Domains.Contracts;

namespace Core.Domains.Groups.Types;

public sealed class GroupStatus : Enumeration
{
    public static readonly GroupStatus Enable = new(1, nameof(Enable));
    public static readonly GroupStatus Disable = new(2, nameof(Disable));

    private GroupStatus(int id, string name) : base(id, name)
    {
    }
}