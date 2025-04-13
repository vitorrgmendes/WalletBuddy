using System.ComponentModel.DataAnnotations.Schema;
using WalletBuddy.Domain.Audit;
using WalletBuddy.Domain.Enums;

namespace WalletBuddy.Domain.Entities;

[Table("users")]
public class User : IAuditableEntity
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Guid UserIdentifier { get; set; }
    public string Role { get; set; } = Roles.MEMBER;
    public DateTime Created_At { get; set; }
    public DateTime Updated_At { get; set; }
    public DateTime? Deleted_At { get; set; }
    public DateTime? LastLogin_At { get; set; }
    public string? RefreshToken { get; set; } = string.Empty;
    public DateTime? RefreshTokenExpiration { get; set; }
}
