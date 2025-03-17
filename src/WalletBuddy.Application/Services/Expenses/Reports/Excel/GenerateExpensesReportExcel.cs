using ClosedXML.Excel;
using WalletBuddy.Domain.Entities;
using WalletBuddy.Domain.Enums;
using WalletBuddy.Domain.Extensions;
using WalletBuddy.Domain.Reports;
using WalletBuddy.Domain.Repositories.Expenses;
using WalletBuddy.Domain.Services.LoggedUser;

namespace WalletBuddy.Application.Services.Expenses.Reports.Excel;

public class GenerateExpensesReportExcel : IGenerateExpensesReportExcel
{
    private readonly IExpensesRepository _repository;
    private readonly ILoggedUser _loggedUser;

    public GenerateExpensesReportExcel(
        IExpensesRepository repository,
        ILoggedUser loggedUser)
    {
        _repository = repository;
        _loggedUser = loggedUser;
    }

    public async Task<byte[]> Execute(DateOnly date)
    {
        var loggedUser = await _loggedUser.Get();

        var expenses = await _repository.GetExpensesByMonth(loggedUser, date);

        if (expenses.Count == 0) return [];

        using var workbook = new XLWorkbook();

        workbook.Author = loggedUser.Name;
        workbook.Style.Font.FontSize = 12;
        workbook.Style.Font.FontName = "Arial";

        var worksheet = workbook.Worksheets.Add(date.ToString("Y"));
        
        InsertHeader(worksheet);
        InsertData(worksheet, expenses);

        var file = new MemoryStream();
        workbook.SaveAs(file);

        return file.ToArray();
    }

    private void InsertHeader(IXLWorksheet worksheet)
    {
        worksheet.Cell("A1").Value = ResourceReportMessages.TITLE;
        worksheet.Cell("B1").Value = ResourceReportMessages.DATE;
        worksheet.Cell("C1").Value = ResourceReportMessages.PAYMENT_TYPE;
        worksheet.Cell("D1").Value = ResourceReportMessages.PRICE;
        worksheet.Cell("E1").Value = ResourceReportMessages.DESCRIPTION;

        worksheet.Cells("A1:E1").Style.Font.Bold = true;

        worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#CC87F9");

        worksheet.Cells("A1:E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    }

    private void InsertData(IXLWorksheet worksheet, List<Expense> expenses)
    {
        var row = 2;
        foreach (var expense in expenses)
        {
            worksheet.Cell($"A{row}").Value = expense.Title;
            worksheet.Cell($"B{row}").Value = expense.Date.ToString("dd/MM/yyyy HH:mm:ss");
            worksheet.Cell($"C{row}").Value = PaymentTypeExtensions.PaymentTypeToString(expense.PaymentType);

            worksheet.Cell($"D{row}").Value = expense.Price;
            worksheet.Cell($"D{row}").Style.NumberFormat.Format = $"{ResourceReportMessages.CURRENCY_SYMBOL} #,##0.00";

            worksheet.Cell($"E{row}").Value = expense.Description;

            row++;
        }

        //worksheet.Columns().AdjustToContents();
    }
}
