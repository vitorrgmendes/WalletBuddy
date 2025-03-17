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

    public async Task DeleteById(long id)
    {
        var expense = await _dbContext.Expenses.FindAsync(id);

        _dbContext.Expenses.Remove(expense!);
    }

    public async Task<List<Expense>> GetAll(User user)
    {
        return await _dbContext.Expenses
            .AsNoTracking()
            .Where(expense => expense.UserId == user.Id)
            .ToListAsync();
    }

    public async Task<Expense?> GetById(User user, long id)
    {
        return await _dbContext.Expenses
            .AsNoTracking()
            .FirstOrDefaultAsync(expense => expense.Id == id && expense.UserId == user.Id);
    }

    public async Task<Expense?> GetByIdForChanges(User user, long id)
    {
        return await _dbContext.Expenses
            .FirstOrDefaultAsync(expense => expense.Id == id && expense.UserId == user.Id);
    }

    public async Task<List<Expense>> GetExpensesByMonth(DateOnly date)
    {
        var startDate = new DateTime(year: date.Year, month: date.Month, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc);

        var daysInMonth = DateTime.DaysInMonth(year: date.Year, month: date.Month);
        var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth, hour: 23, minute: 59, second: 59, kind: DateTimeKind.Utc);

        return await _dbContext
            .Expenses
            .AsNoTracking()
            .Where(expense => expense.Date >= startDate && expense.Date <= endDate)
            .OrderBy(expense => expense.Date)
            .ToListAsync();
    }

    public void Update(Expense expense)
    {
        _dbContext.Expenses.Update(expense);
    }
}
