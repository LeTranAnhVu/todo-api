using System.Text.Json.Serialization;
using Api.Filters;
using Api.Services;
using Application;
using Application.Interfaces;
using Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

var dbConnectionString = builder.Configuration.GetValue<string>("DbConnectionString") ?? "";

services.UseInfraDb(new InfraDbOptions(dbConnectionString));

services.AddApplicationInsightsTelemetry();
services.UseApplication();
services.AddHttpContextAccessor();
services.AddScoped<IUserContextAccessor, UserContextAccessor>();

services.AddControllers(opts =>
{
    opts.Filters.Add<ActivateUserAuthorizationFilter>();
    opts.Filters.Add<ExceptionsFilter>();
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    // options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
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

builder.Services.AddCors(options =>
{
    var allowedOrigins = builder.Configuration.GetSection("Cors").GetSection("AllowedOrigins").Get<string[]>();
    options.AddDefaultPolicy(
        policy =>
        {
            if (allowedOrigins is not null)
            {
                policy.WithOrigins(allowedOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }
        });
});

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
builder.Services.AddHttpLogging(o => { });

builder.Services.AddHealthChecks()
    .AddNpgSql(dbConnectionString);

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpLogging();
app.MapHealthChecks("/healthz");
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();