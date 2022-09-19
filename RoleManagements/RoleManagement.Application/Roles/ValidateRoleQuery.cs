﻿using RoleManagements.Domain.Permissions.Types;
using RoleManagements.Domain.Roles.Types;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Roles;

public sealed record ValidateRoleQuery(RoleId Role, PermissionId RequestedPermission) : Query<bool>;