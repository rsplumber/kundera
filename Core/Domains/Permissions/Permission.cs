using Core.Domains.Permissions.Events;
using Core.Domains.Roles;

namespace Core.Domains.Permissions;

public class Permission : BaseEntity
{
    public Permission()
    {
    }

    internal Permission(string name, IDictionary<string, string>? meta = null)
    {
        Name = name;
        FillMeta(meta);
        AddDomainEvent(new PermissionCreatedEvent(Id));
    }

    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = default!;

    public Dictionary<string, string> Meta { get; set; } = new();

    public void ChangeName(string name) => Name = name;

    private void FillMeta(IDictionary<string, string>? meta)
    {
        if (meta is null) return;
        foreach (var (key, value) in meta)
        {
            Meta.TryAdd(key, value);
        }
    }
}