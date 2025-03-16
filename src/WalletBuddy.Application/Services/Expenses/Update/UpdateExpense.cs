using AutoMapper;
using WalletBuddy.Communication.Requests.Expenses;
using WalletBuddy.Domain.Entities;
using WalletBuddy.Domain.Repositories;
using WalletBuddy.Domain.Repositories.Expenses;
using WalletBuddy.Domain.Services.LoggedUser;
using WalletBuddy.Exception;
using WalletBuddy.Exception.Exception;

namespace WalletBuddy.Application.Services.Expenses.Update;

public class UpdateExpense : IUpdateExpense
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IExpensesRepository _repository;
    private readonly ILoggedUser _loggedUser;

    public UpdateExpense(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        IExpensesRepository repository,
        ILoggedUser loggedUser)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _repository = repository;
        _loggedUser = loggedUser;
    }

    public async Task Execute(long id, RequestExpenseJson request)
    {
        Validate(request);

        var loggedUser = await _loggedUser.Get();

        var expense = await _repository.GetByIdForChanges(loggedUser, id);

        if (expense is null)
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        _mapper.Map(request, expense);
        expense.UpdatedAt = DateTime.UtcNow;

        _repository.Update(expense);

        await _unitOfWork.Commit();
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
