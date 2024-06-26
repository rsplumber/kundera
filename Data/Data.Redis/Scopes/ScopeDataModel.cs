﻿using Redis.OM.Modeling;

namespace Data.Scopes;

[Document(IndexName = "scopes", StorageType = StorageType.Json, Prefixes = new[] { "scope" })]
internal sealed class ScopeDataModel
{
    [RedisIdField] [Indexed] public Guid Id { get; set; } = default!;

    [Indexed]
    [Searchable(PropertyName = "name_searchable")]
    public string Name { get; set; } = default!;

    [Indexed] public string Secret { get; set; } = default!;

    [Indexed] public string Status { get; set; } = default!;

    [Indexed] public List<Guid>? Services { get; set; }

    [Indexed] public List<Guid>? Roles { get; set; }
}