using Microsoft.EntityFrameworkCore;
using ReGuestSiteGreator.Domain.Entities;
using ReGuestSiteGreator.Domain.Enums;

namespace ReGuestSiteGreator.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Partner> Partners => Set<Partner>();
    public DbSet<Plan> Plans => Set<Plan>();
    public DbSet<Block> Blocks => Set<Block>();
    public DbSet<Sitemap> Sitemaps => Set<Sitemap>();
    public DbSet<Page> Pages => Set<Page>();
    public DbSet<PageBlock> PageBlocks => Set<PageBlock>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Email).IsRequired().HasMaxLength(256);
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.Role).HasConversion<int>();
        });

        // Partner
        modelBuilder.Entity<Partner>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
            entity.Property(p => p.CompanyName).HasMaxLength(200);
            entity.HasOne(p => p.User)
                .WithOne(u => u.Partner)
                .HasForeignKey<Partner>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(p => p.Plan)
                .WithMany(pl => pl.Partners)
                .HasForeignKey(p => p.PlanId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Plan
        modelBuilder.Entity<Plan>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Type).HasConversion<int>();
            entity.HasIndex(p => p.Type).IsUnique();
        });

        // Block
        modelBuilder.Entity<Block>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Name).IsRequired().HasMaxLength(200);
            entity.Property(b => b.Template).IsRequired();
            entity.Property(b => b.Meta).HasColumnType("jsonb");
            entity.Property(b => b.DefaultData).HasColumnType("jsonb");
        });

        // Sitemap
        modelBuilder.Entity<Sitemap>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Name).IsRequired().HasMaxLength(200);
            entity.HasOne(s => s.Plan)
                .WithMany(p => p.Sitemaps)
                .HasForeignKey(s => s.PlanId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Page
        modelBuilder.Entity<Page>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Slug).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Title).HasMaxLength(300);
            entity.Property(p => p.MetaTitle).HasMaxLength(300);
            entity.Property(p => p.MetaDescription).HasMaxLength(500);
            entity.Property(p => p.MetaKeywords).HasMaxLength(500);
            entity.Property(p => p.Status).HasConversion<int>();
            entity.HasIndex(p => new { p.SitemapId, p.Slug }).IsUnique();
            entity.HasOne(p => p.Sitemap)
                .WithMany(s => s.Pages)
                .HasForeignKey(p => p.SitemapId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // PageBlock
        modelBuilder.Entity<PageBlock>(entity =>
        {
            entity.HasKey(pb => pb.Id);
            entity.HasIndex(pb => new { pb.PageId, pb.BlockId });
            entity.HasOne(pb => pb.Page)
                .WithMany(p => p.PageBlocks)
                .HasForeignKey(pb => pb.PageId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(pb => pb.Block)
                .WithMany(b => b.PageBlocks)
                .HasForeignKey(pb => pb.BlockId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        var now = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<Plan>().HasData(
            new Plan
            {
                Id = new Guid("10000000-0000-0000-0000-000000000001"),
                Name = "Basic",
                Type = PlanType.Basic,
                Description = "Basic plan with essential features for small websites.",
                CreatedAt = now
            },
            new Plan
            {
                Id = new Guid("10000000-0000-0000-0000-000000000002"),
                Name = "Business",
                Type = PlanType.Business,
                Description = "Business plan with extended features for growing companies.",
                CreatedAt = now
            },
            new Plan
            {
                Id = new Guid("10000000-0000-0000-0000-000000000003"),
                Name = "Premium",
                Type = PlanType.Premium,
                Description = "Premium plan with all features and priority support.",
                CreatedAt = now
            }
        );

        var adminId = new Guid("00000000-0000-0000-0000-000000000001");
        // BCrypt hash for "Admin@123"
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = adminId,
            Email = "admin@reguestsitecreator.com",
            PasswordHash = "$2a$11$2C9OxE9D5fdKyYoqDdG45.sGzq99vvqVjZ3Tey3cUCn0tKyZZppQC",
            Role = UserRole.Admin,
            CreatedAt = now,
            UpdatedAt = now
        });
    }
}
