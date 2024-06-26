﻿using Core.Permissions;
using Core.Roles.Events;

namespace Core.Roles;

public class Role : BaseEntity
{
    public Role()
    {
    }

    internal Role(string name, IDictionary<string, string>? meta = null)
    {
        Name = name;
        FillMeta(meta);
        AddDomainEvent(new RoleCreatedEvent(Id));
    }

    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = default!;

    public List<Permission> Permissions { get; set; } = new();

    public Dictionary<string, string> Meta { get; set; } = new();

    public void ChangeName(string name) => Name = name;

    public void Add(Permission permission)
    {
        if (Has(permission)) return;
        Permissions.Add(permission);
        AddDomainEvent(new RolePermissionAddedEvent(Id, permission.Id));
    }

    public void Remove(Permission permission)
    {
        if (!Has(permission)) return;
        Permissions.Remove(permission);
        AddDomainEvent(new RolePermissionRemovedEvent(Id, permission.Id));
    }

    public bool Has(Permission permission) => Permissions.Any(p => p == permission);

    private void FillMeta(IDictionary<string, string>? meta)
    {
        if (meta is null) return;
        foreach (var (key, value) in meta)
        {
            Meta.TryAdd(key, value);
        }
    }
}