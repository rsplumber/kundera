﻿using System.Net;
using Core.Domains.Contracts;
using Core.Domains.Scopes.Types;
using Core.Domains.Sessions.Events;
using Core.Domains.Users.Types;

namespace Core.Domains.Sessions;

public class Session : Entity
{
    protected Session()
    {
    }

    internal Session(Token token, Token refreshToken, ScopeId scope, UserId user, DateTime expireDate, IPAddress? lastIpAddress = null)
    {
        Id = token;
        RefreshToken = refreshToken;
        Scope = scope;
        User = user;
        ExpiresAt = expireDate;
        LastIpAddress = lastIpAddress ?? IPAddress.None;
        LastUsageDate = DateTime.UtcNow;
        CreatedDate = DateTime.UtcNow;
        AddDomainEvent(new SessionCreatedEvent(Id));
    }

    public Token Id { get; internal set; } = default!;

    public Token RefreshToken { get; internal set; } = default!;

    public ScopeId Scope { get; internal set; }

    public UserId User { get; internal set; }

    public DateTime ExpiresAt { get; internal set; }

    public DateTime LastUsageDate { get; internal set; }

    public IPAddress LastIpAddress { get; internal set; } = default!;

    public DateTime CreatedDate { get; internal set; }


    public void UpdateUsage(DateTime lastUsageDate, IPAddress? ipAddress)
    {
        LastIpAddress = ipAddress ?? IPAddress.None;
        LastUsageDate = lastUsageDate;
    }
}