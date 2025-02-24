using WalletBuddy.Domain.Entities;
using WalletBuddy.Domain.Repositories.Expenses;

namespace WalletBuddy.Infrastructure.Database.Repositories;

internal class ExpensesRepository : IExpensesRepository
{
    private readonly WalletBuddyDbContext _dbContext;

    public ExpensesRepository(WalletBuddyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(Expense expense)
    {
        _dbContext.Expenses.Add(expense);
    }
}
