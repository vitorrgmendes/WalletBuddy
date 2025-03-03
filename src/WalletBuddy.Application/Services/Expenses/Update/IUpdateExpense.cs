using WalletBuddy.Communication.Requests.Expenses;

namespace WalletBuddy.Application.Services.Expenses.Update;

public interface IUpdateExpense
{
    Task Execute(long id, RequestExpenseJson request);
}
