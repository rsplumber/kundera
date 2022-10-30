using System.Reflection;
using Managements.Data;
using Microsoft.AspNetCore.Builder;

namespace Managements.Builder;

internal static class ApplicationBuilderExtension
{
    public static Assembly UseManagements(this IApplicationBuilder app)
    {
        app.ConfigureManagementsData();
        return Assembly.GetExecutingAssembly();
    }
}