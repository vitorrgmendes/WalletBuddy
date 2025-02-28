using AutoMapper;
using WalletBuddy.Communication.Responses.Expenses;
using WalletBuddy.Domain.Repositories.Expenses;

namespace WalletBuddy.Application.Services.Expenses.GetAll;

public class GetAllExpenses : IGetAllExpenses
{
    private readonly IExpensesRepository _repository;
    private readonly IMapper _mapper;

    public GetAllExpenses(IExpensesRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseExpensesJson> Execute()
    {
        var expenses = await _repository.GetAll();

        return new ResponseExpensesJson
        {
            Expenses = _mapper.Map<List<ResponseShortExpenseJson>>(expenses)
        };
    }
}
