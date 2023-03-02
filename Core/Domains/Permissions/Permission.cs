using Core.Domains.Permissions.Events;

namespace Core.Domains.Permissions;

public class Permission : BaseEntity
{
    protected Permission()
    {
    }

    internal Permission(string name, IDictionary<string, string>? meta = null)
    {
        Name = name;
        FillMeta(meta);
        AddDomainEvent(new PermissionCreatedEvent(Id));
    }

    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; internal set; } = default!;

    public IDictionary<string, string> Meta { get; internal set; } = new Dictionary<string, string>();

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