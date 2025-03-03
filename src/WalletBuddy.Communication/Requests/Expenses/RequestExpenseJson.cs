using WalletBuddy.Communication.Enums;

namespace WalletBuddy.Communication.Requests.Expenses;

public class RequestExpenseJson
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal Price { get; set; }
    public PaymentType PaymentType { get; set; }
}
