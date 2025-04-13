namespace WalletBuddy.Domain.Entities;

public class AuditLog
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Entity { get; set; } = string.Empty;
    public string Operation { get; set; } = string.Empty;
    public long? UserId { get; set; }
    public string? EntityBefore { get; set; }
    public string? EntityAfter { get; set; }
    public string? Changes { get; set; }    
}

