using Authorization.Infrastructure;
using Authorization.Web.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);


var configuration = builder.Configuration;
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization(configuration);
builder.Services.AddAuthorizationWeb(configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureAuthorization(configuration);
app.ConfigureAuthorizationWeb(configuration);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();