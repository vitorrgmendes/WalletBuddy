using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Security.Claims;
using System.Text.Json;
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

    public DbSet<Expense> Expenses { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Global filter: remove registers with the column Deleted_At != null
        modelBuilder.Entity<User>().HasQueryFilter(user => user.Deleted_At == null);
    }

    #region: Audit
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var modifiedEntities = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added || e.State == EntityState.Deleted)
            .ToList();       

        foreach (var entity in modifiedEntities)
        {
            var auditLog = new AuditLog
            {
                Date = DateTime.UtcNow,
                Entity = entity.Entity.GetType().Name,
                Operation = entity.State.ToString(),
                UserId = GetLoggedUserId(),
                EntityBefore = GetEntityBefore(entity),
                EntityAfter = GetEntityAfter(entity),
                Changes = GetChanges(entity)
            };

            AuditLogs.Add(auditLog);
        }
        
        return await base.SaveChangesAsync(cancellationToken);
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
        if (entity.State == EntityState.Modified || entity.State == EntityState.Deleted)
        {
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

        return null;
    }

    private string? GetEntityAfter(EntityEntry entity)
    {
        if (entity.State == EntityState.Modified || entity.State == EntityState.Added)
        {
            var currentValues = entity.CurrentValues.Properties.ToDictionary(
                p => p.Name,
                p => entity.CurrentValues[p]
            );

            return JsonSerializer.Serialize(currentValues, new JsonSerializerOptions
            {
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        return null;
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
