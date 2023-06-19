using Core.Permissions.Events;
using Core.Services;

namespace Core.Permissions;

public class Permission : BaseEntity
{
    public Permission()
    {
    }

    internal Permission(Service service, string name, IDictionary<string, string>? meta = null)
    {
        Name = $"{service.Name}_{name}";
        Service = service;
        FillMeta(meta);
        AddDomainEvent(new PermissionCreatedEvent(Id));
    }

    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = default!;

    public Dictionary<string, string> Meta { get; set; } = new();

    public Service Service { get; set; } = default!;

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