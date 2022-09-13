﻿using RoleManagements.Domain.Permissions.Events;
using RoleManagements.Domain.Permissions.Exceptions;
using RoleManagements.Domain.Permissions.Types;
using Tes.Domain.Contracts;

namespace RoleManagements.Domain.Permissions;

public class Permission : AggregateRoot<PermissionId>
{
    private readonly Dictionary<string, string> _meta;

    protected Permission()
    {
    }

    private Permission(PermissionId id) : base(id)
    {
        _meta = new Dictionary<string, string>();
        AddDomainEvent(new PermissionCreatedEvent(id));
    }

    public static async Task<Permission> CreateAsync(Name name, IPermissionRepository repository)
    {
        var id = PermissionId.From(name);
        var exists = await repository.ExistsAsync(id);
        if (exists)
        {
            throw new PermissionAlreadyExistsException(name);
        }

        return new Permission(id);
    }

    public IReadOnlyDictionary<string, string> Meta => _meta;

    public void AddMeta(string key, string value)
    {
        _meta.TryAdd(key, value);
    }

    public void RemoveMeta(string key)
    {
        if (GetMetaValue(key) is null) return;
        _meta.Remove(key);
    }

    public string? GetMetaValue(string key)
    {
        _meta.TryGetValue(key, out var value);
        return value;
    }
}