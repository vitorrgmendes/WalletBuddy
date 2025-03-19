using CommonUtilities.Test.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WalletBuddy.Domain.Entities;
using WalletBuddy.Domain.Security.Cryptography;
using WalletBuddy.Domain.Security.Tokens;
using WalletBuddy.Infrastructure.Database;
using WebApi.Test.Resources;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public UserIdentityManager User_Member { get; private set; } = default!;
    public UserIdentityManager User_Admin { get; private set; } = default!;
    public ExpenseIdentityManager Expense { get; private set; } = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<WalletBuddyDbContext>(config => 
                {
                    config.UseInMemoryDatabase("InMemoryDbForTesting");
                    config.UseInternalServiceProvider(provider);
                });

                var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<WalletBuddyDbContext>();
                var passwordEncrypter = scope.ServiceProvider.GetRequiredService<IPasswordEncrypter>();
                var tokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

                StartDatabase(dbContext, passwordEncrypter, tokenGenerator);                
            });
    }

    private void StartDatabase(
        WalletBuddyDbContext dbContext, 
        IPasswordEncrypter passwordEncrypter, 
        IAccessTokenGenerator tokenGenerator)
    {
        var memberUser = AddMemberUser(dbContext, passwordEncrypter, tokenGenerator);
        AddExpenses(dbContext, memberUser);

        dbContext.SaveChanges();
    }

    private User AddMemberUser(
        WalletBuddyDbContext dbContext, 
        IPasswordEncrypter passwordEncrypter, 
        IAccessTokenGenerator tokenGenerator)
    {
        var user = UserBuilder.Build();

        var password = user.Password;
        user.Password = passwordEncrypter.Encrypt(password);

        dbContext.Users.Add(user);

        var token = tokenGenerator.Generate(user);

        User_Member = new UserIdentityManager(user, password, token);

        return user;
    }

    private void AddExpenses(WalletBuddyDbContext dbContext, User user)
    {
        var expense = ExpenseBuilder.Build(user);

        dbContext.Expenses.Add(expense);

        Expense = new ExpenseIdentityManager(expense);
    }
}
