using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using System.Reflection;
using WalletBuddy.Application.Services.Expenses.Reports.Pdf.Colors;
using WalletBuddy.Application.Services.Expenses.Reports.Pdf.Fonts;
using WalletBuddy.Domain.Entities;
using WalletBuddy.Domain.Extensions;
using WalletBuddy.Domain.Reports;
using WalletBuddy.Domain.Repositories.Expenses;
using WalletBuddy.Domain.Services.LoggedUser;
using Font = MigraDoc.DocumentObjectModel.Font;

namespace WalletBuddy.Application.Services.Expenses.Reports.Pdf;

public class GenerateExpensesReportPdf : IGenerateExpensesReportPdf
{
    private readonly IExpensesRepository _repository;
    private readonly ILoggedUser _loggedUser;

    private const string pathLogo = "Services\\Expenses\\Reports\\Pdf\\Logo";
    private const string logoFile = "logo.png";

    private const int HEIGHT_ROW_EXPENSE_TABLE = 25;

    public GenerateExpensesReportPdf(
        IExpensesRepository repository,
        ILoggedUser loggedUser)
    {
        _repository = repository;
        _loggedUser = loggedUser;

        GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
    }

    public async Task<byte[]> Execute(DateOnly date)
    {
        var loggedUser = await _loggedUser.Get();

        var expenses = await _repository.GetExpensesByMonth(loggedUser, date);

        if (expenses.Count == 0) return [];

        var document = CreateDocument(loggedUser.Name, date);
        var page = CreatePage(document);

        // Header
        CreateHeader(loggedUser.Name, page);

        // Total Value Paragraph
        CreateParagraphTotalValue(page, expenses, date);

        // Insert expense table
        foreach (var expense in expenses)
        {
            var table = CreateExpenseTable(page);
            AddExpenseTableInformation(table, expense);
        }

        // Render document
        return RenderDocument(document);
    }

    private Document CreateDocument(string author, DateOnly date)
    { 
        var document = new Document();
        document.Info.Title = $"{ResourceReportMessages.EXPENSES_FOR} {date:Y}";
        document.Info.Author = author;

        var style = document.Styles["Normal"];
        style!.Font.Name = FontHelper.RALEWAY_REGULAR;

        return document;
    }

    private Section CreatePage(Document document)
    { 
        var section = document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();

        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.LeftMargin = 40;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.TopMargin = 80;
        section.PageSetup.BottomMargin = 80;

        return section;
    }

    private void CreateHeader(string name, Section page)
    {
        var table = page.AddTable();
        table.AddColumn();
        table.AddColumn("300");

        var row = table.AddRow();

        var assembly = Assembly.GetExecutingAssembly();
        var directoryName = Path.GetDirectoryName(assembly.Location);
        var pathFile = Path.Combine(directoryName!, pathLogo, logoFile);

        row.Cells[0].AddImage(pathFile);

        row.Cells[1].AddParagraph($"Hey, {name}!");
        row.Cells[1].Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 16 };
        row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
    }

    private void CreateParagraphTotalValue(Section page, List<Expense> expenses, DateOnly date)
    {
        var paragraph = page.AddParagraph();

        paragraph.Format.SpaceBefore = "40";
        paragraph.Format.SpaceAfter = "40";

        var title = string.Format(ResourceReportMessages.TOTAL_SPENT_IN, date.ToString("Y"));

        paragraph.AddFormattedText(title, new Font { Name = FontHelper.RALEWAY_REGULAR, Size = 15 });

        paragraph.AddLineBreak();

        var totalExpenses = expenses.Sum(expense => expense.Price);
        paragraph.AddFormattedText($"{ResourceReportMessages.CURRENCY_SYMBOL} {totalExpenses:f2}",
                                    new Font { Name = FontHelper.WORKSANS_BLACK, Size = 50 });
    }

    private Table CreateExpenseTable(Section page)
    { 
        var table = page.AddTable();
        table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

        return table;
    }

    private void AddExpenseTableInformation(Table table, Expense expense)
    {
        // Header row
        var row = table.AddRow();
        row.Height = HEIGHT_ROW_EXPENSE_TABLE;

        AddExpenseTitle(row.Cells[0], expense.Title);
        AddHeaderForPrice(row.Cells[3]);

        // Expense information row
        row = table.AddRow();
        row.Height = HEIGHT_ROW_EXPENSE_TABLE;

        // Column Date
        row.Cells[0].AddParagraph(expense.Date.ToString("D"));
        SetBaseStyleForExpenseInformation(row.Cells[0]);
        row.Cells[0].Format.LeftIndent = 20;

        // Column Time
        row.Cells[1].AddParagraph(expense.Date.ToString("t"));
        SetBaseStyleForExpenseInformation(row.Cells[1]);

        // Column Payment Type
        row.Cells[2].AddParagraph(PaymentTypeExtensions.PaymentTypeToString(expense.PaymentType));
        SetBaseStyleForExpenseInformation(row.Cells[2]);

        // Column Price
        AddExpensePriceInformation(row.Cells[3], expense.Price);

        // Description row
        if (!string.IsNullOrWhiteSpace(expense.Description))
        {
            AddExpenseDescriptionRow(table, expense.Description);
            row.Cells[3].MergeDown = 1;
        }

        // White Space
        table.AddRow().Height = 30;
    }

    private void AddExpenseTitle(Cell cell, string expenseTitle)
    {
        cell.AddParagraph(expenseTitle);
        cell.Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 14, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.RED_LIGHT;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2;
        cell.Format.LeftIndent = 20;
    }

    private void AddHeaderForPrice(Cell cell)
    {
        cell.AddParagraph(ResourceReportMessages.PRICE);
        cell.Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 14, Color = ColorsHelper.WHITE };
        cell.Shading.Color = ColorsHelper.RED_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void SetBaseStyleForExpenseInformation(Cell cell)
    {
        cell.Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 12, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.GREEN_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddExpensePriceInformation(Cell cell, decimal expensePrice)
    {
        cell.AddParagraph($"- {ResourceReportMessages.CURRENCY_SYMBOL} {expensePrice:f2}");
        cell.Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 14, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.WHITE;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddExpenseDescriptionRow(Table table, string expenseDescription)
    {
        var descriptionRow = table.AddRow();
        descriptionRow.Height = HEIGHT_ROW_EXPENSE_TABLE;
        descriptionRow.Cells[0].AddParagraph(expenseDescription);
        descriptionRow.Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 10, Color = ColorsHelper.BLACK };
        descriptionRow.Shading.Color = ColorsHelper.GREEN_LIGHT;
        descriptionRow.VerticalAlignment = VerticalAlignment.Center;
        descriptionRow.Cells[0].Format.LeftIndent = 20;
        descriptionRow.Cells[0].MergeRight = 2;
    }

    private byte[] RenderDocument(Document document)
    {
        var renderer = new PdfDocumentRenderer { Document = document};
        renderer.RenderDocument();

        using var file = new MemoryStream();
        renderer.PdfDocument.Save(file);

        return file.ToArray();
    }
}
