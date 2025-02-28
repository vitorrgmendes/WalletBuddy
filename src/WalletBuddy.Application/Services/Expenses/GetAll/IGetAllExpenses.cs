using WalletBuddy.Communication.Responses.Expenses;

namespace WalletBuddy.Application.Services.Expenses.GetAll;

public interface IGetAllExpenses
{
    Task<ResponseExpensesJson> Execute();
}
