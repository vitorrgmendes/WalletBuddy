using WalletBuddy.Communication.Enums;

namespace WalletBuddy.Communication.Responses.Expenses;

public class ResponseExpenseJson
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public decimal Price { get; set; }
    public PaymentType PaymentType { get; set; }
    public IList<TagEnum> Tags { get; set; } = [];
}
