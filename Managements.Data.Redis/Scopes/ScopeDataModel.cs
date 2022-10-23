﻿using Redis.OM.Modeling;

namespace Managements.Data.Scopes;

[Document(IndexName = "scopes", StorageType = StorageType.Json, Prefixes = new[] {"scope"})]
public class ScopeDataModel
{
    [RedisIdField] [Indexed] public Guid Id { get; set; }

    [Indexed] public string Name { get; set; }

    [Indexed] public string Secret { get; set; }

    [Indexed] public string Status { get; set; }

    [Indexed] public List<Guid> Services { get; set; }

    [Indexed] public List<Guid> Roles { get; set; }
}