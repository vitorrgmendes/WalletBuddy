using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        //AddLogging(services, configuration);

        AddSecurity(services);
        AddToken(services, configuration);
        AddApiKey(services, configuration);
        AddRepositories(services);       

        if (!configuration.IsTestEnvironment())
            AddDbContext(services, configuration);
    }

    private static void AddLogging(IServiceCollection services, IConfiguration configuration)
    {
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
