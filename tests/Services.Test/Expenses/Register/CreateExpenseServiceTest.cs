using CommonUtilities.Test.Entities;
using CommonUtilities.Test.LoggedUser;
using CommonUtilities.Test.Mapper;
using CommonUtilities.Test.Repositories;
using CommonUtilities.Test.Requests;
using WalletBuddy.Application.Services.Expenses.Create;
using WalletBuddy.Domain.Entities;
using WalletBuddy.Exception;
using WalletBuddy.Exception.Exception;

namespace Services.Test.Expenses.Register;

public class CreateExpenseServiceTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestExpenseCreateJsonBuilder.Build();
        var service = CreateService(loggedUser);

        var result = await service.Execute(request);

        Assert.NotNull(result);
        Assert.Equal(request.Title, result.Title);
    }

    [Fact]
    public async Task Error_Empty_Title()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestExpenseCreateJsonBuilder.Build();
        request.Title = string.Empty;

        var service = CreateService(loggedUser);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(async () => await service.Execute(request));

        Assert.Single(exception.GetErrors());
        Assert.Contains(ResourceErrorMessages.TITLE_REQUIRED, exception.GetErrors());
    }

    private CreateExpense CreateService(User user)
    {
        var mapper = MapperBuilder.Build();
        var repository = new ExpenseRepositoryBuilder().Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new CreateExpense(repository, unitOfWork, mapper, loggedUser);
    }
}
