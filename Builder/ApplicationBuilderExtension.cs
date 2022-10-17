using Auth.Builder;
using Managements.Builder;
using Microsoft.AspNetCore.Builder;

namespace Builder;

public static class ApplicationBuilderExtension
{
    public static void UseKundera(this IApplicationBuilder app)
    {
        app.ConfigureAuth();
        app.ConfigureManagements();
    }
}