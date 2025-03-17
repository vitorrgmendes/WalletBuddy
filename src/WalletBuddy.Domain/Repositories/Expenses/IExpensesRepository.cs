using WalletBuddy.Domain.Entities;

namespace WalletBuddy.Domain.Repositories.Expenses;

public interface IExpensesRepository
{
    Task Add(Expense expense);

    Task<List<Expense>> GetAll(User user);

    Task<Expense?> GetById(User user, long id);

    Task DeleteById(long id);

    void Update(Expense expense);

    Task<Expense?> GetByIdForChanges(User user, long id);

    Task<List<Expense>> GetExpensesByMonth(User user, DateOnly date);
}
