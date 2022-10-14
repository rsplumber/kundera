﻿using Application.Users;
using Domain;
using Domain.Users;
using Kite.Web.Requests;

namespace Web.Api.Users;

public record SuspendUserStatusRequest(string? Reason) : IWebRequest
{
    public SuspendUserCommand ToCommand(Guid userId) => new(UserId.From(userId), Text.From(Reason!));
}

public record BlockUserStatusRequest(string Reason) : IWebRequest
{
    public BlockUserCommand ToCommand(Guid userId) => new(UserId.From(userId), Text.From(Reason));
}