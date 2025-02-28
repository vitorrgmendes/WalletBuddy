using AutoMapper;
using WalletBuddy.Communication.Responses.Expenses;
using WalletBuddy.Domain.Repositories.Expenses;
using WalletBuddy.Exception;
using WalletBuddy.Exception.Exception;

namespace WalletBuddy.Application.Services.Expenses.GetById;

public class GetExpenseById : IGetExpenseById
{
    private readonly IExpensesRepository _repository;
    private readonly IMapper _mapper;

    public GetExpenseById(IExpensesRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseExpenseJson> Execute(long id)
    {
        var expense = await _repository.GetById(id);

        if (expense is null)
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        return _mapper.Map<ResponseExpenseJson>(expense);
    }
}
