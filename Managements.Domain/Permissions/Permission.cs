using Kite.Domain.Contracts;
using Managements.Domain.Permissions.Events;
using Managements.Domain.Permissions.Exceptions;

namespace Managements.Domain.Permissions;

public class Permission : AggregateRoot<PermissionId>
{
    private string _name;
    private readonly Dictionary<string, string> _meta = new();

    protected Permission()
    {
    }

    private Permission(Name name) : base(PermissionId.Generate())
    {
        _name = name;
        AddDomainEvent(new PermissionCreatedEvent(Id));
    }

    public static async Task<Permission> FromAsync(Name name, IPermissionRepository repository)
    {
        var exists = await repository.ExistsAsync(name);
        if (exists)
        {
            throw new PermissionAlreadyExistsException(name);
        }

        return new Permission(name);
    }

    public Name Name => _name;

    public IReadOnlyDictionary<string, string> Meta => _meta;

    public void ChangeName(Name name) => _name = name;

    public void AddMeta(string key, string value)
    {
        RemoveMeta(key);
        _meta.Add(key, value);
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