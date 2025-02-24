using WalletBuddy.Domain.Entities;

namespace WalletBuddy.Domain.Repositories.Expenses;

public interface IExpensesRepository
{
    Task Add(Expense expense);
}
