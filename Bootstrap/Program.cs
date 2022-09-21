using Authentication.Infrastructure;
using Authorization.Infrastructure;
using RoleManagement.Builder;
using RoleManagement.Web.Api.Extensions;
using Users.Builder;
using Web.Api;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddUsers(configuration);
builder.Services.AddRoleManagement(configuration);
builder.Services.AddAuthorization(configuration);
builder.Services.AddAuthenticationService(configuration);
builder.Services.AddKunderaWeb(configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureUsers(configuration);
app.ConfigureRoleManagementWeb(configuration);
app.ConfigureAuthorization(configuration);
app.ConfigureAuthorization(configuration);
app.ConfigureKunderaWeb(configuration);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();