using Authentication.Infrastructure;
using Authentication.Web.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthenticationService(configuration);
builder.Services.AddAuthenticationWeb(configuration);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureAuthenticationService(configuration);
app.ConfigureAuthenticationWeb(configuration);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();