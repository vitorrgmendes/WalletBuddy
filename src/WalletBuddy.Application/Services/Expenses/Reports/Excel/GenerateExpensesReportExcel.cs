using ClosedXML.Excel;
using WalletBuddy.Domain.Entities;
using WalletBuddy.Domain.Enums;
using WalletBuddy.Domain.Reports;
using WalletBuddy.Domain.Repositories.Expenses;

namespace WalletBuddy.Application.Services.Expenses.Reports.Excel;

public class GenerateExpensesReportExcel : IGenerateExpensesReportExcel
{
    private readonly IExpensesRepository _repository;

    public GenerateExpensesReportExcel(IExpensesRepository repository)
    {
        _repository = repository;
    }

    public async Task<byte[]> Execute(DateOnly date)
    {
        var expenses = await _repository.GetExpensesByMonth(date);

        if (expenses.Count == 0) return [];

        using var workbook = new XLWorkbook();

        workbook.Author = "Vitor Mendes";
        workbook.Style.Font.FontSize = 12;
        workbook.Style.Font.FontName = "Arial";

        var worksheet = workbook.Worksheets.Add(date.ToString("Y"));
        
        InsertHeader(worksheet);
        InsertData(worksheet, expenses);

        var file = new MemoryStream();
        workbook.SaveAs(file);

        return file.ToArray();
    }

    private string ConvertPaymentType(PaymentType payment)
    {
        return payment switch
        {
            PaymentType.Cash => ResourceReportMessages.CASH,
            PaymentType.CreditCard => ResourceReportMessages.CREDIT_CARD,
            PaymentType.DebitCard => ResourceReportMessages.DEBIT_CARD,
            PaymentType.EletronicTransfer => ResourceReportMessages.ELETRONIC_TRANSFER,
            _ => string.Empty
        };
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
            worksheet.Cell($"C{row}").Value = ConvertPaymentType(expense.PaymentType);

            worksheet.Cell($"D{row}").Value = expense.Price;
            worksheet.Cell($"D{row}").Style.NumberFormat.Format = $"{ResourceReportMessages.CURRENCY_SYMBOL} #,##0.00";

            worksheet.Cell($"E{row}").Value = expense.Description;

            row++;
        }

        //worksheet.Columns().AdjustToContents();
    }
}
