using WalletBuddy.Domain.Repositories;
using WalletBuddy.Infrastructure.Database;

namespace WalletBuddy.Infrastructure;

internal class UnitOfWork : IUnitOfWork
{
    private readonly WalletBuddyDbContext _dbContext;

    public UnitOfWork(WalletBuddyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Commit() => await _dbContext.SaveChangesAsync();
}
