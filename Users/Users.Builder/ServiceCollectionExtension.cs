﻿using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tes.CQRS.InMemory;
using Tes.Serializer.Microsoft;
using Users.Data;

namespace Users.Builder;

public static class ServiceCollectionExtension
{
    public static void AddUsers(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMicrosoftSerializer(configuration);
        var assemblies = new[]
        {
            Assembly.Load("Users.Application"),
            Assembly.Load("Users.Data")
        };
        services.AddCqrsInMemory(configuration, assemblies);
        services.AddDataLayer(configuration);
    }
}