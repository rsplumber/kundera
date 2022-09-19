﻿using Tes.CQRS.Contracts;
using Users.Domain;
using Users.Domain.Users;

namespace Users.Application.Users;

public sealed record RevokeUserRoleCommand(UserId User, params RoleId[] Roles) : Command;