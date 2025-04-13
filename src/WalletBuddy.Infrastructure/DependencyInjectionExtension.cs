using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.AwsCloudWatch;
using Serilog.Sinks.SystemConsole.Themes;
using WalletBuddy.Domain.Repositories;
using WalletBuddy.Domain.Repositories.Expenses;
using WalletBuddy.Domain.Repositories.Users;
using WalletBuddy.Domain.Security.ApiKey;
using WalletBuddy.Domain.Security.Constants;
using WalletBuddy.Domain.Security.Cryptography;
using WalletBuddy.Domain.Security.Tokens;
using WalletBuddy.Domain.Services.LoggedUser;
using WalletBuddy.Infrastructure.Database;
using WalletBuddy.Infrastructure.Database.Repositories;
using WalletBuddy.Infrastructure.Extensions;
using WalletBuddy.Infrastructure.Security.ApiKey;
using WalletBuddy.Infrastructure.Security.Tokens;
using WalletBuddy.Infrastructure.Services.LoggedUser;

namespace WalletBuddy.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddLogging(services);

        AddSecurity(services);
        AddToken(services, configuration);
        AddApiKey(services, configuration);
        AddRepositories(services);       

        if (!configuration.IsTestEnvironment())
            AddDbContext(services, configuration);
    }

    private static void AddLogging(IServiceCollection services)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        var customTheme = new SystemConsoleTheme(new Dictionary<ConsoleThemeStyle, SystemConsoleThemeStyle>
        {
            [ConsoleThemeStyle.LevelInformation] = new SystemConsoleThemeStyle { Foreground = ConsoleColor.Green },
            [ConsoleThemeStyle.LevelWarning] = new SystemConsoleThemeStyle { Foreground = ConsoleColor.Yellow },
            [ConsoleThemeStyle.LevelError] = new SystemConsoleThemeStyle { Foreground = ConsoleColor.Red },
            [ConsoleThemeStyle.LevelFatal] = new SystemConsoleThemeStyle { Foreground = ConsoleColor.DarkRed },
        });

        var outputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss zzz}] [{Level}] {Message:lj}{NewLine}{Exception}";

        // Serilog Default Configuration
        var loggerConfig = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .WriteTo.Console(theme: customTheme, outputTemplate: outputTemplate);

        // Add AWS CloudWatch Sink Configuration only for Production
        if (environment is "Production")
        {
            var cloudWatchOptions = new CloudWatchSinkOptions
            {
                LogGroupName = "WalletBuddyLogs",
                TextFormatter = new Serilog.Formatting.Compact.CompactJsonFormatter(),
                MinimumLogEventLevel = Serilog.Events.LogEventLevel.Error,
                CreateLogGroup = true
            };
            loggerConfig.WriteTo.AmazonCloudWatch(cloudWatchOptions, new Amazon.CloudWatchLogs.AmazonCloudWatchLogsClient());
        }

        Log.Logger = loggerConfig.CreateLogger();
        
        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog(Log.Logger);
        });

        Log.Information("Application starting...");
    }

    private static void AddApiKey(IServiceCollection services, IConfiguration configuration)
    {
        var apiKey = configuration.GetValue<string>(SecurityConstants.API_KEY_PATH_NAME);
        services.AddScoped<IApiKeyValidation>(config => new ApiKeyValidation(apiKey!));
    }

    private static void AddSecurity(IServiceCollection services)
    {
        services.AddScoped<IPasswordEncrypter, Security.Cryptography.BCrypt>();
        services.AddScoped<ILoggedUser, LoggedUser>();
    }

    private static void AddToken(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>(SecurityConstants.JWT_TOKEN_EXPIRATION_PATH);
        var signingKey = configuration.GetValue<string>(SecurityConstants.JWT_SIGNINGKEY_PATH);
        var refreshTokenExpirationDays = configuration.GetValue<double>(SecurityConstants.REFRESH_TOKEN_EXPIRATION_PATH);

        services.AddScoped<IAccessTokenGenerator>(config => new JwtTokenGenerator(expirationTimeMinutes, signingKey!, refreshTokenExpirationDays));
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IExpensesRepository, ExpensesRepository>();   
        services.AddScoped<IUserRepository, UserRepository>();
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection");

        services.AddDbContext<WalletBuddyDbContext>(options =>
        options.UseNpgsql(connectionString)
               .UseLowerCaseNamingConvention());
    }
}
