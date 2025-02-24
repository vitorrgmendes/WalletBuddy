using WalletBuddy.Communication.Requests.Expenses;
using WalletBuddy.Communication.Responses.Expenses;
using WalletBuddy.Domain.Entities;
using WalletBuddy.Domain.Repositories;
using WalletBuddy.Domain.Repositories.Expenses;
using WalletBuddy.Exception.Exception;

namespace WalletBuddy.Application.Services.Expenses.Create;
public class CreateExpense : ICreateExpense
{
    private readonly IExpensesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateExpense(IExpensesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseExpenseCreatedJson> Execute(RequestExpenseCreateJson request)
    {
        Validate(request);

        var expense = new Expense
        {
            Title = request.Title,
            Description = request.Description,
            Date = request.Date,
            Price = request.Price,
            PaymentType = (Domain.Enums.PaymentType)request.PaymentType
        };

        await _repository.Add(expense);
        await _unitOfWork.Commit();

        return new ResponseExpenseCreatedJson { Title = expense.Title};
    }

    private void Validate(RequestExpenseCreateJson request)
    {
        var validator = new CreateExpenseValidator();

        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }        
    }
}
