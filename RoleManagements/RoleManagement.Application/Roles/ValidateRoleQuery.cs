﻿using RoleManagements.Domain.Permissions.Types;
using RoleManagements.Domain.Roles.Types;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Roles;

public sealed record ValidateRoleQuery(RoleId RoleId, PermissionId RequestedPermission) : Query<bool>;