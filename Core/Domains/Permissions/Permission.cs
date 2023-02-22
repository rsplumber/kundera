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
        if (meta is not null)
        {
            foreach (var (key, value) in meta)
            {
                Meta.Add(key, value);
            }
        }

        AddDomainEvent(new PermissionCreatedEvent(Id));
    }

    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; internal set; } = default!;

    public IDictionary<string, string> Meta { get; internal set; } = new Dictionary<string, string>();

    public void ChangeName(string name) => Name = name;
}