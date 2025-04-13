using WalletBuddy.Domain.Audit;
using WalletBuddy.Domain.Enums;

namespace WalletBuddy.Domain.Entities;

public class Tag : IAuditableEntity
{
    public long Id { get; set; }
    public TagEnum Value { get; set; }

    public long ExpenseId { get; set; }
    public Expense Expense { get; set; } = default!;
}
