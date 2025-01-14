using WalletBuddy.Communication.Requests.Expenses;
using WalletBuddy.Communication.Responses.Expenses;

namespace WalletBuddy.Application.Services.Expenses;
public class CreateExpense
{
    public ResponseExpenseCreatedJson Execute(RequestExpenseCreateJson request)
    {
        return new ResponseExpenseCreatedJson();
    }
}
