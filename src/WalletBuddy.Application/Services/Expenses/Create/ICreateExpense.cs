using WalletBuddy.Communication.Requests.Expenses;
using WalletBuddy.Communication.Responses.Expenses;

namespace WalletBuddy.Application.Services.Expenses.Create;

public interface ICreateExpense
{
    ResponseExpenseCreatedJson Execute(RequestExpenseCreateJson request);
}
