using WalletBuddy.Domain.Entities;

namespace WalletBuddy.Domain.Repositories.Expenses;

public interface IExpensesRepository
{
    void Add(Expense expense);
}
