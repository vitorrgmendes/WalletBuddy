using AutoMapper;
using WalletBuddy.Communication.Requests.Expenses;
using WalletBuddy.Communication.Responses.Expenses;
using WalletBuddy.Domain.Entities;
using WalletBuddy.Domain.Repositories;
using WalletBuddy.Domain.Repositories.Expenses;
using WalletBuddy.Domain.Services.LoggedUser;
using WalletBuddy.Exception.Exception;

namespace WalletBuddy.Application.Services.Expenses.Create;
public class CreateExpense : ICreateExpense
{
    private readonly IExpensesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public CreateExpense(
        IExpensesRepository repository, 
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        ILoggedUser loggedUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }

    public async Task<ResponseExpenseCreatedJson> Execute(RequestExpenseJson request)
    {
        Validate(request);

        var loggedUser = await _loggedUser.Get();

        var expense = _mapper.Map<Expense>(request);
        expense.UserId = loggedUser.Id;
        expense.CreatedAt = DateTime.UtcNow;
        expense.UpdatedAt = expense.CreatedAt;

        await _repository.Add(expense);
        await _unitOfWork.Commit();

        return _mapper.Map<ResponseExpenseCreatedJson>(expense);
    }

    private void Validate(RequestExpenseJson request)
    {
        var validator = new ExpenseValidator();

        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }        
    }
}
