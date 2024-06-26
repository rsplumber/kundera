﻿using Redis.OM.Modeling;

namespace Data.Roles;

[Document(IndexName = "roles", StorageType = StorageType.Json, Prefixes = new[] { "role" })]
internal sealed class RoleDataModel
{
    [RedisIdField] [Indexed] public Guid Id { get; set; }

    [Indexed]
    [Searchable(PropertyName = "name_searchable")]
    public string Name { get; set; } = default!;

    public Dictionary<string, string>? Meta { get; set; }

    [Indexed] public List<Guid>? Permissions { get; set; }
}