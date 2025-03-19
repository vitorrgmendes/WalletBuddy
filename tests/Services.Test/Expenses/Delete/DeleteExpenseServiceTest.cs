using CommonUtilities.Test.Entities;
using CommonUtilities.Test.LoggedUser;
using CommonUtilities.Test.Repositories;
using WalletBuddy.Application.Services.Expenses.Delete;
using WalletBuddy.Domain.Entities;
using WalletBuddy.Exception;
using WalletBuddy.Exception.Exception;

namespace Services.Test.Expenses.Delete;

public class DeleteExpenseServiceTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(loggedUser);

        var service = CreateService(loggedUser, expense);

        var exception = await Record.ExceptionAsync(async () => await service.Execute(expense.Id));

        Assert.Null(exception);
    }

    [Fact]
    public async Task Error_Expense_Not_Found()
    {
        var loggedUser = UserBuilder.Build();

        var service = CreateService(loggedUser);

        var exception = await Assert.ThrowsAsync<NotFoundException>(async () => await service.Execute(id: 777));

        Assert.Single(exception.GetErrors());
        Assert.Contains(ResourceErrorMessages.EXPENSE_NOT_FOUND, exception.GetErrors());
    }

    private DeleteExpense CreateService(User user, Expense? expense = null)
    {
        var repository = new ExpenseRepositoryBuilder().GetById(user, expense).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new DeleteExpense(repository, unitOfWork, loggedUser);
    }
}
