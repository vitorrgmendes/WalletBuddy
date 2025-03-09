using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WalletBuddy.Infrastructure.Database;

namespace WalletBuddy.Infrastructure.Migrations;

public class DataBaseMigration
{
    public async static Task MigrateDatabase(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<WalletBuddyDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}
