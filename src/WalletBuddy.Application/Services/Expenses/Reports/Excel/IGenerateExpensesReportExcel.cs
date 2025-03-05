namespace WalletBuddy.Application.Services.Expenses.Reports.Excel;

public interface IGenerateExpensesReportExcel
{
    Task<byte[]> Execute(DateOnly month);
}
