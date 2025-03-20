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

    public ExpenseRepositoryBuilder GetById(User user, Expense? expense)
    {
        if (expense is not null)
            _repository.Setup(repository => repository.GetById(user, expense.Id)).ReturnsAsync(expense);

        return this;
    }

    public ExpenseRepositoryBuilder GetByIdForChanges(User user, Expense? expense)
    {
        if (expense is not null)
            _repository.Setup(repository => repository.GetByIdForChanges(user, expense.Id)).ReturnsAsync(expense);

        return this;
    }

    public ExpenseRepositoryBuilder GetExpensesByMonth(User user, List<Expense> expenses)
    {
        _repository.Setup(repository => repository.GetExpensesByMonth(user, It.IsAny<DateOnly>())).ReturnsAsync(expenses);

        return this;
    }

    public IExpensesRepository Build() => _repository.Object;
}
