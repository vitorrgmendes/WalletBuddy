using Microsoft.EntityFrameworkCore;
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

    public async Task Add(Expense expense)
    {
        await _dbContext.Expenses.AddAsync(expense);
    }

    public async Task<bool> DeleteById(long id)
    {
        return await _dbContext.Expenses.FirstOrDefaultAsync(expense => expense.Id == id) is Expense expenseToDelete 
            && _dbContext.Expenses.Remove(expenseToDelete) is not null;
    }

    public async Task<List<Expense>> GetAll()
    {
        return await _dbContext.Expenses.AsNoTracking().ToListAsync();
    }

    public async Task<Expense?> GetById(long id)
    {
        return await _dbContext.Expenses.AsNoTracking().FirstOrDefaultAsync(expense => expense.Id == id);
    }

    public async Task<Expense?> GetByIdForChanges(long id)
    {
        return await _dbContext.Expenses.FirstOrDefaultAsync(expense => expense.Id == id);
    }

    public void Update(Expense expense)
    {
        _dbContext.Expenses.Update(expense);
    }
}
