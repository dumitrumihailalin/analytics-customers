using AnalyticsCustomers.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsCustomers.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Store> Stores => Set<Store>();
    public DbSet<Analytic> Analytics => Set<Analytic>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<ApiKey> ApiKeys => Set<ApiKey>();
    public DbSet<SubscriptionKey> SubscriptionKeys => Set<SubscriptionKey>();
    public DbSet<Plan> Plans => Set<Plan>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>(e =>
        {
            e.HasIndex(u => u.Email).IsUnique();
            e.HasOne(u => u.Organization).WithMany(o => o.Users)
             .HasForeignKey(u => u.OrganizationId)
             .IsRequired(false);
        });

        builder.Entity<Store>(e =>
        {
            e.HasOne(s => s.Organization).WithMany(o => o.Stores)
             .HasForeignKey(s => s.OrganizationId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Analytic>(e =>
        {
            e.Property(a => a.Price).HasColumnType("numeric(12,2)");
            e.HasOne(a => a.Store).WithMany(s => s.Analytics)
             .HasForeignKey(a => a.StoreId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(a => new { a.StoreId, a.RecordedAt });
            e.HasIndex(a => new { a.StoreId, a.ProductId });
        });

        builder.Entity<Subscription>(e =>
        {
            e.HasOne(s => s.Organization).WithMany()
             .HasForeignKey(s => s.OrganizationId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ApiKey>(e =>
        {
            e.HasIndex(k => k.KeyHash).IsUnique();
            e.HasOne(k => k.Store).WithMany(s => s.ApiKeys)
             .HasForeignKey(k => k.StoreId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<SubscriptionKey>(e =>
        {
            e.HasIndex(k => k.Key).IsUnique();
        });
    }
}
