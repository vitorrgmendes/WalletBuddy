using CommonUtilities.Test.Entities;
using CommonUtilities.Test.LoggedUser;
using CommonUtilities.Test.Repositories;
using WalletBuddy.Application.Services.Expenses.Reports.Excel;
using WalletBuddy.Domain.Entities;
using WalletBuddy.Domain.Enums;

namespace Services.Test.Expenses.Reports.Excel;

public class GenerateExpensesReportExcelServiceTest
{
    [Fact]
    public async Task Success()
    {
        User loggedUser = UserBuilder.Build(Roles.ADMIN);
        var expenses = ExpenseBuilder.Collection(loggedUser);

        var service = CreateService(loggedUser, expenses);

        var result = await service.Execute(DateOnly.FromDateTime(DateTime.Today));

        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task Success_Empty()
    {
        User loggedUser = UserBuilder.Build(Roles.ADMIN);

        var service = CreateService(loggedUser, []);

        var result = await service.Execute(DateOnly.FromDateTime(DateTime.Today));

        Assert.Empty(result);
    }

    private GenerateExpensesReportExcel CreateService(User user, List<Expense> expenses)
    {
        var repository = new ExpenseRepositoryBuilder().GetExpensesByMonth(user, expenses).Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GenerateExpensesReportExcel(repository, loggedUser);
    }
}
