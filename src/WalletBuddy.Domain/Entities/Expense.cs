using System.ComponentModel.DataAnnotations.Schema;
using WalletBuddy.Domain.Enums;

namespace WalletBuddy.Domain.Entities;

[Table("expenses")]
public class Expense
{
    [Column("id")]
    public long Id { get; set; }

    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Column("description")]
    public string? Description { get; set; }

    [Column("date")]
    public DateTime Date { get; set; }

    [Column("price")]
    public decimal Price { get; set; }

    [Column("paymenttype")]
    public PaymentType PaymentType { get; set; }
}
