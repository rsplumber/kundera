﻿using ValueOf;

namespace Managements.Domain.Users.Types;

public sealed class UserId : ValueOf<Guid, UserId>
{
    public static UserId Generate() => From(Guid.NewGuid());
}