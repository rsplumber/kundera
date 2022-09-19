using RoleManagement.Builder;
using RoleManagement.Web.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRoleManagement(configuration);
builder.Services.AddRoleManagementWeb(configuration);

var app = builder.Build();
app.ConfigureRoleManagement(configuration);
app.ConfigureRoleManagementWeb(configuration);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

await app.RunAsync();

await app.WaitForShutdownAsync();