using CommonUtilities.Test.Entities;
using CommonUtilities.Test.LoggedUser;
using CommonUtilities.Test.Mapper;
using CommonUtilities.Test.Repositories;
using CommonUtilities.Test.Requests;
using WalletBuddy.Application.Services.Expenses.Update;
using WalletBuddy.Domain.Entities;
using WalletBuddy.Exception.Exception;
using WalletBuddy.Exception;
using WalletBuddy.Domain.Enums;

namespace Services.Test.Expenses.Update;

public class UpdateExpenseServiceTest 
{
    [Fact]
    public async Task Success()
    {
        User loggedUser = UserBuilder.Build();
        var request = RequestExpenseJsonBuilder.Build();
        var expense = ExpenseBuilder.Build(loggedUser);

        var service = CreateService(loggedUser, expense);

        var exception = await Record.ExceptionAsync(async () => await service.Execute(expense.Id, request));

        Assert.Null(exception);

        Assert.Equal(request.Title, expense.Title);
        Assert.Equal(request.Description, expense.Description);
        Assert.Equal(request.Date, expense.Date);
        Assert.Equal(request.Price, expense.Price);
        Assert.Equal((PaymentType)request.PaymentType, expense.PaymentType);
    }

    [Fact]
    public async Task Error_Empty_Title()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestExpenseJsonBuilder.Build();
        request.Title = string.Empty;

        var service = CreateService(loggedUser);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(async () => await service.Execute(id: 777, request));

        Assert.Single(exception.GetErrors());
        Assert.Contains(ResourceErrorMessages.TITLE_REQUIRED, exception.GetErrors());
    }

    [Fact]
    public async Task Error_Expense_Not_Found()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestExpenseJsonBuilder.Build();

        var service = CreateService(loggedUser);

        var exception = await Assert.ThrowsAsync<NotFoundException>(async () => await service.Execute(id: 777, request));

        Assert.Single(exception.GetErrors());
        Assert.Contains(ResourceErrorMessages.EXPENSE_NOT_FOUND, exception.GetErrors());
    }

    private UpdateExpense CreateService(User user, Expense? expense = null)
    {
        var mapper = MapperBuilder.Build();
        var repository = new ExpenseRepositoryBuilder().GetByIdForChanges(user, expense).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new UpdateExpense(unitOfWork, mapper, repository, loggedUser);
    }
}
