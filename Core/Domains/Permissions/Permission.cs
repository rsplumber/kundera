using Core.Domains.Permissions.Events;
using Core.Domains.Permissions.Types;

namespace Core.Domains.Permissions;

public class Permission : AggregateRoot
{
    protected Permission()
    {
    }

    internal Permission(Name name, IDictionary<string, string>? meta = null)
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

    public PermissionId Id { get; set; } = PermissionId.Generate();

    public Name Name { get; internal set; } = default!;

    public IDictionary<string, string> Meta { get; internal set; } = new Dictionary<string, string>();

    public void ChangeName(Name name) => Name = name;
}