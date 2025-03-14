using Microsoft.EntityFrameworkCore;
using WalletBuddy.Domain.Entities;

namespace WalletBuddy.Infrastructure.Database;

public class WalletBuddyDbContext : DbContext
{
    public WalletBuddyDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Expense> Expenses { get; set; }
    public DbSet<User> Users { get; set; }
}
