using FastFoodDeals.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastFoodDeals.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Deal> Deals => Set<Deal>();
    public DbSet<DealClick> DealClicks => Set<DealClick>();
    public DbSet<FeedbackEvent> FeedbackEvents => Set<FeedbackEvent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Deal>(entity =>
        {
            entity.HasKey(e => e.Id);

            // Mapeo de precisión decimal para MySQL (evita truncamiento o warnings)
            entity.Property(e => e.OriginalPrice)
                .HasPrecision(10, 2);

            entity.Property(e => e.DiscountPrice)
                .HasPrecision(10, 2);

            entity.Property(e => e.Brand)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.ExternalId)
                .HasMaxLength(150);

            entity.Property(e => e.Category)
                .HasMaxLength(50);

            entity.Property(e => e.LocationZone)
                .HasMaxLength(50);

            // Índices para optimizar las consultas del scraper y recomendador
            entity.HasIndex(e => new { e.Brand, e.ExternalId });
            entity.HasIndex(e => e.IsActive);
            entity.HasIndex(e => e.LocationZone);
        });

        modelBuilder.Entity<DealClick>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.DealId);
        });

        modelBuilder.Entity<FeedbackEvent>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Comment).HasMaxLength(500);
        });
    }
}
