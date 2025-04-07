using CommonUtilities.Test.Entities;
using CommonUtilities.Test.LoggedUser;
using CommonUtilities.Test.Mapper;
using CommonUtilities.Test.Repositories;
using WalletBuddy.Application.Services.Expenses.GetById;
using WalletBuddy.Communication.Enums;
using WalletBuddy.Domain.Entities;
using WalletBuddy.Exception;
using WalletBuddy.Exception.Exception;

namespace Services.Test.Expenses.GetById;

public class GetExpenseByIdServiceTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(loggedUser);

        var service = CreateService(loggedUser, expense);

        var result = await service.Execute(expense.Id);

        Assert.NotNull(result);
        Assert.Equal(expense.Id, result.Id);
        Assert.Equal(expense.Title, result.Title);
        Assert.Equal(expense.Description, result.Description);
        Assert.Equal(expense.Date, result.Date);
        Assert.Equal(expense.Price, result.Price);
        Assert.Equal((PaymentType)expense.PaymentType, result.PaymentType);
        
        Assert.NotNull(result.Tags);
        Assert.NotEmpty(result.Tags);
        Assert.Equivalent(expense.Tags.Select(tag => (TagEnum)tag.Value), result.Tags);
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

    private GetExpenseById CreateService(User user, Expense? expense = null)
    {
        var mapper = MapperBuilder.Build();
        var repository = new ExpenseRepositoryBuilder().GetById(user, expense).Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GetExpenseById(repository, mapper, loggedUser);
    }
}
