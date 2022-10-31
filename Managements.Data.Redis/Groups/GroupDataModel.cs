using Redis.OM.Modeling;

namespace Managements.Data.Groups;

[Document(IndexName = "groups", StorageType = StorageType.Json, Prefixes = new[] {"groups"})]
internal sealed class GroupDataModel
{
    [RedisIdField] [Indexed] public Guid Id { get; set; }

    [Indexed] public string Name { get; set; }

    public string Description { get; set; }

    [Indexed] public Guid? Parent { get; set; }

    [Indexed] public string Status { get; set; }
    
    [Indexed] public List<Guid> Children { get; set; }

    public DateTime StatusChangeDate { get; set; }

    [Indexed] public List<Guid> Roles { get; set; }
}