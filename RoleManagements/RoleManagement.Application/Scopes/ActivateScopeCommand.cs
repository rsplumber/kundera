﻿using RoleManagements.Domain.Scopes.Types;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Scopes;

public sealed record ActivateScopeCommand(ScopeId Id) : Command;