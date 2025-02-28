using WalletBuddy.Communication.Responses.Expenses;

namespace WalletBuddy.Application.Services.Expenses.GetById;

public interface IGetExpenseById
{
    Task<ResponseExpenseJson> Execute(long id);
}
