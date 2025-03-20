using CommonUtilities.Test.Entities;
using CommonUtilities.Test.LoggedUser;
using CommonUtilities.Test.Mapper;
using CommonUtilities.Test.Repositories;
using WalletBuddy.Application.Services.Expenses.GetAll;
using WalletBuddy.Domain.Entities;

namespace Services.Test.Expenses.GetAll;

public class GetAllExpensesServiceTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var expenses = ExpenseBuilder.Collection(loggedUser);

        var service = CreateService(loggedUser, expenses);

        var result = await service.Execute();

        Assert.NotNull(result);
        Assert.NotNull(result.Expenses);
        Assert.NotEmpty(result.Expenses);

        foreach (var expense in result.Expenses)
        {
            Assert.True(expense.Id > 0);
            Assert.False(string.IsNullOrEmpty(expense.Title));
            Assert.True(expense.Price > 0);
        }
    }

    private GetAllExpenses CreateService(User user, List<Expense> expenses)
    {
        var mapper = MapperBuilder.Build();
        var repository = new ExpenseRepositoryBuilder().GetAll(user, expenses).Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GetAllExpenses(repository, mapper, loggedUser);
    }
}
