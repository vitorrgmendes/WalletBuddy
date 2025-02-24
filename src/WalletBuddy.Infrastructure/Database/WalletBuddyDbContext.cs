using Microsoft.EntityFrameworkCore;
using WalletBuddy.Domain.Entities;

namespace WalletBuddy.Infrastructure.Database;

internal class WalletBuddyDbContext : DbContext
{
    public WalletBuddyDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Expense> Expenses { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    // Dados de conexão com o PostgreSQL
    //    var connectionString = "";

    //    // Use Npgsql para configurar a conexão
    //    optionsBuilder.UseNpgsql(connectionString);
    //}
}
