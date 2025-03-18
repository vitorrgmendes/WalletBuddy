using Moq;
using WalletBuddy.Domain.Entities;
using WalletBuddy.Domain.Repositories.Expenses;

namespace CommonUtilities.Test.Repositories;

public class ExpenseRepositoryBuilder
{
    private readonly Mock<IExpensesRepository> _repository;

    public ExpenseRepositoryBuilder()
    {
        _repository = new Mock<IExpensesRepository>();
    }

    public ExpenseRepositoryBuilder GetAll(User user, List<Expense> expenses)
    { 
        _repository.Setup(repository => repository.GetAll(user)).ReturnsAsync(expenses);

        return this;
    }

    public IExpensesRepository Build() => _repository.Object;
}
