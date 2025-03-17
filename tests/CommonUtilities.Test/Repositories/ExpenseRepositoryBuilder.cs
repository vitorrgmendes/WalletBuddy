using Moq;
using WalletBuddy.Domain.Repositories.Expenses;

namespace CommonUtilities.Test.Repositories;

public class ExpenseRepositoryBuilder
{
    private readonly Mock<IExpensesRepository> _repository;

    public ExpenseRepositoryBuilder()
    {
        _repository = new Mock<IExpensesRepository>();
    }

    public IExpensesRepository Build() => _repository.Object;
}
