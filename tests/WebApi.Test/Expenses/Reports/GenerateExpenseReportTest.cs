using System.Net;
using System.Net.Mime;

namespace WebApi.Test.Expenses.Reports;

public class GenerateExpenseReportTest : WalletBuddyClassFixture
{
    private const string URI = "api/expenses/report";

    private readonly string _adminToken;
    private readonly string _memberToken;
    private readonly DateTime _expenseDate;

    public GenerateExpenseReportTest(CustomWebApplicationFactory customWebApplicationFactory) : base(customWebApplicationFactory)
    {
        _adminToken = customWebApplicationFactory.User_Admin.GetToken();
        _memberToken = customWebApplicationFactory.User_Member.GetToken();
        _expenseDate = customWebApplicationFactory.Expense_Admin.GetDate();
    }

    [Fact]
    public async Task Success_Pdf()
    {
        var result = await DoGet(requestUri: $"{URI}/pdf?date={_expenseDate.ToString("MM/yyyy")}", token: _adminToken);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        Assert.NotNull(result.Content.Headers.ContentType);
        Assert.Equal(MediaTypeNames.Application.Pdf, result.Content.Headers.ContentType.MediaType);
    }

    [Fact]
    public async Task Success_Excel()
    {
        var result = await DoGet(requestUri: $"{URI}/excel?date={_expenseDate.ToString("MM/yyyy")}", token: _adminToken);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        Assert.NotNull(result.Content.Headers.ContentType);
        Assert.Equal(MediaTypeNames.Application.Octet, result.Content.Headers.ContentType.MediaType);
    }

    [Fact]
    public async Task Error_Forbidden_Pdf()
    {
        var result = await DoGet(requestUri: $"{URI}/pdf?date={_expenseDate.ToString("MM/yyyy")}", token: _memberToken);

        Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
    }

    [Fact]
    public async Task Error_Forbidden_Excel()
    {
        var result = await DoGet(requestUri: $"{URI}/excel?date={_expenseDate.ToString("MM/yyyy")}", token: _memberToken);

        Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
    }
}
