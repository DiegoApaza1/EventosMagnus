using Hangfire;
using Hangfire.MemoryStorage;
using Magnus.Api.Middlewares;
using Magnus.Infrastructure.Adapters.Persistence.DbContexts;
using Magnus.Infrastructure.Adapters.Persistence.Repositories;
using Magnus.Infrastructure.Adapters.Services;
using Microsoft.EntityFrameworkCore;
using Magnus.Domain.Interfaces;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (connectionString?.StartsWith("postgresql://") == true)
{
    var uri = new Uri(connectionString);
    var port = uri.Port > 0 ? uri.Port : 5432;
    var userInfo = uri.UserInfo.Split(':');
    connectionString = $"Host={uri.Host};Port={port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
}

Console.WriteLine($"[DEBUG] Environment: {builder.Environment.EnvironmentName}");
Console.WriteLine($"[DEBUG] Using connection to: {connectionString?.Split(';')[0]}");

builder.Services.AddDbContext<MagnusDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(Assembly.Load("Magnus.Application")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddScoped<IReportService, ExcelReportService>();

builder.Services.AddHangfire(config => config.UseMemoryStorage());
builder.Services.AddHangfireServer();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAntiforgery();
builder.Services.AddSwaggerGen(c =>

{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EventosMagnus API",
        Version = "v1",
        Description = "API para la gestión de eventos"
    });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese 'Bearer {token}'"
    };
    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, new List<string>() }
    });
});

var jwtIssuer = builder.Configuration["Jwt:Issuer"]
                 ?? throw new InvalidOperationException("Jwt:Issuer no configurado");
var jwtAudience = builder.Configuration["Jwt:Audience"]
                   ?? throw new InvalidOperationException("Jwt:Audience no configurado");
var jwtKey = builder.Configuration["Jwt:Key"]
             ?? throw new InvalidOperationException("Jwt:Key no configurado");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!)),
        ClockSkew = TimeSpan.FromSeconds(30)
    };
});

var app = builder.Build();

// Swagger habilitado en todos los entornos (incluido Production)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventosMagnus API V1");
    c.RoutePrefix = string.Empty; // Swagger en la raíz
});

app.UseGlobalExceptionHandling();
app.UseHangfireDashboard("/hangfire");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapControllers();

//ruta raiz
app.MapGet("/", () => Results.Redirect("/swagger"))
.ExcludeFromDescription();

app.MapGet("/health", () => Results.Ok(new 
{ 
    status = "healthy", 
    timestamp = DateTime.UtcNow,
    environment = app.Environment.EnvironmentName
}))
.WithName("HealthCheck")
.WithTags("Health");

app.Run();

public partial class Program { }