using Managements.Data;
using Microsoft.AspNetCore.Builder;

namespace Managements.Builder;

internal static class ApplicationBuilderExtension
{
    public static void UseManagements(this IApplicationBuilder app)
    {
        app.ConfigureManagementsData();
    }
}