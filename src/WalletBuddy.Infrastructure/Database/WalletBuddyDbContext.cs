using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Security.Claims;
using System.Text.Json;
using WalletBuddy.Domain.Audit;
using WalletBuddy.Domain.Entities;

namespace WalletBuddy.Infrastructure.Database;

public class WalletBuddyDbContext : DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WalletBuddyDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) 
        : base(options) 
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tag>().ToTable("tags");

        // Global filter: remove registers with the column Deleted_At != null
        modelBuilder.Entity<User>().HasQueryFilter(user => user.Deleted_At == null);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var preAuditData = PrepareAuditData();

        var result = await base.SaveChangesAsync(cancellationToken);

        await PersistAuditLogsAsync(preAuditData, cancellationToken);

        return result;
    }

    #region: DbSets

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

    #endregion

    #region: Audit

        private List<(EntityEntry Entry, string Operation, string? Before, string? Changes)> PrepareAuditData()
        {
            return ChangeTracker.Entries()
                .Where(e =>
                    e.Entity is IAuditableEntity &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted) &&
                    e.Entity is not AuditLog)
                .Select(entry => (
                    Entry: entry,
                    Operation: entry.State.ToString(),
                    Before: GetEntityBefore(entry),
                    Changes: GetChanges(entry)
                ))
                .ToList();
        }

        private async Task PersistAuditLogsAsync(List<(EntityEntry Entry, string Operation, string? Before, string? Changes)> preSaveAuditData,
                                                 CancellationToken cancellationToken)
        {
            var auditLogs = preSaveAuditData.Select(data => new AuditLog
            {
                Date = DateTime.UtcNow,
                Entity = data.Entry.Entity.GetType().Name,
                Operation = data.Operation,
                UserId = GetLoggedUserId(),
                EntityBefore = data.Before,
                EntityAfter = GetEntityAfter(data.Entry, data.Operation),
                Changes = data.Changes
            }).ToList();

            if (auditLogs.Count != 0)
            {
                AuditLogs.AddRange(auditLogs);
                await base.SaveChangesAsync(cancellationToken);
            }
        }

        private long? GetLoggedUserId()
        {
            var loggedUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.SerialNumber);

            long? parsedUserId = null;

            if (!string.IsNullOrEmpty(loggedUserId) 
                && long.TryParse(loggedUserId, out var result))
                parsedUserId = result;

            return parsedUserId;
        }

        private string? GetEntityBefore(EntityEntry entity)
        {
            if (entity.State is not (EntityState.Modified or EntityState.Deleted))
                return null;

            var originalValues = entity.OriginalValues.Properties.ToDictionary(
                p => p.Name,
                p => entity.OriginalValues[p]
            );

            return JsonSerializer.Serialize(originalValues, new JsonSerializerOptions
            {
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        private string? GetEntityAfter(EntityEntry entry, string operation)
        {
            if (operation is not "Modified" and not "Added")
                return null;

            var currentValues = entry.CurrentValues.Properties.ToDictionary(
                p => p.Name,
                p => entry.CurrentValues[p]
            );

            return JsonSerializer.Serialize(currentValues, new JsonSerializerOptions
            {
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        private string? GetChanges(EntityEntry entity)
        {
            var before = new Dictionary<string, object>();
            var after = new Dictionary<string, object>();

            foreach (var property in entity.OriginalValues.Properties)
            {
                var beforeValue = entity.OriginalValues[property];
                var afterValue = entity.CurrentValues[property];

                if (!Equals(beforeValue, afterValue))
                {
                    before[property.Name] = beforeValue!;
                    after[property.Name] = afterValue!;
                }
            }

            if (before.Count == 0 && after.Count == 0)
            {
                return null;
            }

            var changes = new
            {
                Before = before,
                After = after
            };

            return JsonSerializer.Serialize(changes, new JsonSerializerOptions
            {
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

    #endregion

}