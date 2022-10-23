using Redis.OM.Modeling;

namespace Managements.Data.UserGroups;

[Document(IndexName = "groups", StorageType = StorageType.Json, Prefixes = new[] {"groups"})]
internal sealed class UserGroupDataModel
{
    [RedisIdField] [Indexed] public Guid Id { get; set; }

    [Indexed] public string Name { get; set; }

    public string Description { get; set; }

    [Indexed] public Guid? Parent { get; set; }

    [Indexed] public string Status { get; set; }

    public DateTime StatusChangedDate { get; set; }

    [Indexed] public List<Guid> Roles { get; set; }
}