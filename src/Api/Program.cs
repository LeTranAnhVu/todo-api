using Api.Filters;
using Api.Services;
using Application;
using Application.Interfaces;
using Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services.UseInfraDb(new InfraDbOptions()
{
    DbConnectionString = builder.Configuration.GetValue<string>("DbConnectionString") ?? "",
});

services.UseApplication();
services.AddHttpContextAccessor();
services.AddScoped<IUserContextAccessor, UserContextAccessor>();

services.AddControllers(opts =>
{
    opts.Filters.Add<ActivateUserAuthorizationFilter>();
    opts.Filters.Add<ExceptionsFilter>();
});

services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var auth0 = builder.Configuration.GetSection("Auth0");

    options.Authority = auth0.GetValue<string>("Authority");
    options.Audience = auth0.GetValue<string>("Audience");
});

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();