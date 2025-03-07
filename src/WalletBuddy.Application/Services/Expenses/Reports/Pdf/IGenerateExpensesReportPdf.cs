using WalletBuddy.Domain.Entities;

namespace WalletBuddy.Application.Services.Expenses.Reports.Pdf;

public interface IGenerateExpensesReportPdf
{
    Task<byte[]> Execute(DateOnly date);
}
