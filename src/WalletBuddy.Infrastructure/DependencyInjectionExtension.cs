using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WalletBuddy.Domain.Repositories;
using WalletBuddy.Domain.Repositories.Expenses;
using WalletBuddy.Domain.Repositories.Users;
using WalletBuddy.Domain.Security.Cryptography;
using WalletBuddy.Domain.Security.Tokens;
using WalletBuddy.Infrastructure.Database;
using WalletBuddy.Infrastructure.Database.Repositories;
using WalletBuddy.Infrastructure.Security.Tokens;

namespace WalletBuddy.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext(services, configuration);
        AddRepositories(services);
        AddSecurity(services);
        AddToken(services, configuration);
    }

    private static void AddSecurity(IServiceCollection services)
    {
        services.AddScoped<IPasswordEncripter, Security.Cryptography.BCrypt>();
    }

    private static void AddToken(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpiresMinutes");
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");
        var refreshTokenExpirationDays = configuration.GetValue<double>("Settings:Jwt:RefreshTokenExpiresDays");

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
        services.AddDbContext<WalletBuddyDbContext>(config => config.UseNpgsql(connectionString)
        .UseLowerCaseNamingConvention());
    }
}
