using WalletBuddy.Communication.Requests.Expenses;
using WalletBuddy.Communication.Responses.Expenses;

namespace WalletBuddy.Application.Services.Expenses.Create;
public class CreateExpense
{
    public ResponseExpenseCreatedJson Execute(RequestExpenseCreateJson request)
    {
        Validate(request);

        return new ResponseExpenseCreatedJson();
    }

    private void Validate(RequestExpenseCreateJson request)
    {
        var validator = new CreateExpenseValidator();

        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ArgumentException();
        }        
    }
}
