using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WalletBuddy.Api.Filters;
using WalletBuddy.Api.Filters.CustomAttributes;
using WalletBuddy.Api.Middleware;
using WalletBuddy.Application;
using WalletBuddy.Domain.Security.Constants;
using WalletBuddy.Infrastructure;
using WalletBuddy.Infrastructure.Migrations;
using WalletBuddy.Infrastructure.Extensions;
using WalletBuddy.Domain.Security.Tokens;
using WalletBuddy.Api.Token;
using Serilog;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using WalletBuddy.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

// Add Swagger Token info
builder.Services.AddSwaggerGen(config => 
{
    config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer abc123def456'",
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        Type = SecuritySchemeType.ApiKey
    });

    config.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            { 
                Reference = new OpenApiReference
                { 
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// Exceptions & API Key Filter
builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));
builder.Services.AddScoped<ApiKeyAuthFilter>();

// Dependency Injection
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

// Serilog Logging
builder.Host.UseSerilog();

// Token Provider
builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();
builder.Services.AddHttpContextAccessor();

// JWT Authorization
var signingKey = builder.Configuration.GetValue<string>(SecurityConstants.JWT_SIGNINGKEY_PATH);
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config => 
{
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey!))
    };
});

// Health Check service
builder.Services.AddHealthChecks().AddDbContextCheck<WalletBuddyDbContext>();

var app = builder.Build();

// Health Check route
app.MapHealthChecks("/health", new HealthCheckOptions
{
    AllowCachingResponses = false,
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
    }
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Languages Middleware
app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

// Auth
app.UseAuthentication();
app.UseAuthorization();

// Serilog Requests Middleware
app.UseSerilogRequestLogging(options =>
{
    options.GetLevel = (httpContext, elapsed, ex) =>
    {
        if (ex != null || elapsed > 500)
            return Serilog.Events.LogEventLevel.Warning;

        return Serilog.Events.LogEventLevel.Information;
    };
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
        diagnosticContext.Set("RequestPath", httpContext.Request.Path);
    };
});

app.MapControllers();

// Auto Run Migrations
if (!builder.Configuration.IsTestEnvironment())
    await MigrateDatabase();

Log.Information("Application running.");
app.Run();

async Task MigrateDatabase()
{
    try
    {
        Log.Information("Starting database migration...");
        await using var scope = app.Services.CreateAsyncScope();
        await DataBaseMigration.MigrateDatabase(scope.ServiceProvider);
        Log.Information("Database migration completed.");
    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "An error occurred while migrating the database.");
    }
}

public partial class Program { }
