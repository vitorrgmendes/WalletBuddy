using WalletBuddy.Domain.Repositories;
using WalletBuddy.Domain.Repositories.Expenses;
using WalletBuddy.Exception;
using WalletBuddy.Exception.Exception;

namespace WalletBuddy.Application.Services.Expenses.Delete;

public class DeleteExpense : IDeleteExpense
{
    private readonly IExpensesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteExpense(IExpensesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(long id)
    {
        var result = await _repository.DeleteById(id);

        if (result is false)
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        await _unitOfWork.Commit();
    }
}
