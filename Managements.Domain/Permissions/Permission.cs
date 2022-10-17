using Kite.Domain.Contracts;
using Managements.Domain.Permissions.Events;
using Managements.Domain.Permissions.Exceptions;

namespace Managements.Domain.Permissions;

public class Permission : AggregateRoot<PermissionId>
{
    private readonly Dictionary<string, string> _meta = new();

    protected Permission()
    {
    }

    private Permission(PermissionId id) : base(id)
    {
        AddDomainEvent(new PermissionCreatedEvent(id));
    }

    public static async Task<Permission> FromAsync(Name name, IPermissionRepository repository)
    {
        var id = PermissionId.From(name.Value);
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