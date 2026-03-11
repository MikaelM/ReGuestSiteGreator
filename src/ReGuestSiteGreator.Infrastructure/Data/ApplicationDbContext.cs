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

        // BCrypt hash for "Admin@123"
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = new Guid("00000000-0000-0000-0000-000000000001"),
            Email = "admin@reguestsitecreator.com",
            PasswordHash = "$2a$11$2C9OxE9D5fdKyYoqDdG45.sGzq99vvqVjZ3Tey3cUCn0tKyZZppQC",
            Role = UserRole.Admin,
            CreatedAt = now,
            UpdatedAt = now
        },
        // BCrypt hash for "s3cUrePassw0rdF0rAdmin123$"
        new User
        {
            Id = new Guid("00000000-0000-0000-0000-000000000002"),
            Email = "admin",
            PasswordHash = "$2a$11$VaYCYAUH0DMR3GwWU0D1HOazM8lv/z0XcE/r1ROlP9euDOhqYumvS",
            Role = UserRole.Admin,
            CreatedAt = now,
            UpdatedAt = now
        });

        // Blocks
        var headerId  = new Guid("20000000-0000-0000-0000-000000000001");
        var footerId  = new Guid("20000000-0000-0000-0000-000000000002");
        var block1Id  = new Guid("20000000-0000-0000-0000-000000000003");
        var block2Id  = new Guid("20000000-0000-0000-0000-000000000004");
        var block3Id  = new Guid("20000000-0000-0000-0000-000000000005");
        var block4Id  = new Guid("20000000-0000-0000-0000-000000000006");

        modelBuilder.Entity<Block>().HasData(
            new Block { Id = headerId, Name = "Header",       Template = "<p>Header</p>",       Style = "", Script = "", Meta = "{}", DefaultData = "{}", CreatedAt = now, UpdatedAt = now },
            new Block { Id = footerId, Name = "Block Footer", Template = "<p>Block Footer</p>", Style = "", Script = "", Meta = "{}", DefaultData = "{}", CreatedAt = now, UpdatedAt = now },
            new Block { Id = block1Id, Name = "Block 1",      Template = "<p>Block 1</p>",      Style = "", Script = "", Meta = "{}", DefaultData = "{}", CreatedAt = now, UpdatedAt = now },
            new Block { Id = block2Id, Name = "Block 2",      Template = "<p>Block 2</p>",      Style = "", Script = "", Meta = "{}", DefaultData = "{}", CreatedAt = now, UpdatedAt = now },
            new Block { Id = block3Id, Name = "Block 3",      Template = "<p>Block 3</p>",      Style = "", Script = "", Meta = "{}", DefaultData = "{}", CreatedAt = now, UpdatedAt = now },
            new Block { Id = block4Id, Name = "Block 4",      Template = "<p>Block 4</p>",      Style = "", Script = "", Meta = "{}", DefaultData = "{}", CreatedAt = now, UpdatedAt = now }
        );

        // Sitemaps
        var basicSitemapId   = new Guid("30000000-0000-0000-0000-000000000001");
        var premiumSitemapId = new Guid("30000000-0000-0000-0000-000000000002");

        modelBuilder.Entity<Sitemap>().HasData(
            new Sitemap { Id = basicSitemapId,   Name = "Basic Sitemap",   Description = "", PlanId = new Guid("10000000-0000-0000-0000-000000000001"), CreatedAt = now, UpdatedAt = now },
            new Sitemap { Id = premiumSitemapId, Name = "Premium Sitemap", Description = "", PlanId = new Guid("10000000-0000-0000-0000-000000000003"), CreatedAt = now, UpdatedAt = now }
        );

        // Pages
        var basicHomeId          = new Guid("40000000-0000-0000-0000-000000000001");
        var basicContactId       = new Guid("40000000-0000-0000-0000-000000000002");
        var premiumHomeId        = new Guid("40000000-0000-0000-0000-000000000003");
        var premiumContactId     = new Guid("40000000-0000-0000-0000-000000000004");
        var premiumFaqId         = new Guid("40000000-0000-0000-0000-000000000005");

        modelBuilder.Entity<Page>().HasData(
            new Page { Id = basicHomeId,      SitemapId = basicSitemapId,   Name = "Home",       Slug = "home",       Title = "Home",       Description = "", Content = "", MetaTitle = "", MetaDescription = "", MetaKeywords = "", Status = PageStatus.Draft, SortOrder = 1, IsHomePage = true,  CreatedAt = now, UpdatedAt = now },
            new Page { Id = basicContactId,   SitemapId = basicSitemapId,   Name = "Contact Us", Slug = "contact-us", Title = "Contact Us", Description = "", Content = "", MetaTitle = "", MetaDescription = "", MetaKeywords = "", Status = PageStatus.Draft, SortOrder = 2, IsHomePage = false, CreatedAt = now, UpdatedAt = now },
            new Page { Id = premiumHomeId,    SitemapId = premiumSitemapId, Name = "Home",       Slug = "home",       Title = "Home",       Description = "", Content = "", MetaTitle = "", MetaDescription = "", MetaKeywords = "", Status = PageStatus.Draft, SortOrder = 1, IsHomePage = true,  CreatedAt = now, UpdatedAt = now },
            new Page { Id = premiumContactId, SitemapId = premiumSitemapId, Name = "Contact Us", Slug = "contact-us", Title = "Contact Us", Description = "", Content = "", MetaTitle = "", MetaDescription = "", MetaKeywords = "", Status = PageStatus.Draft, SortOrder = 2, IsHomePage = false, CreatedAt = now, UpdatedAt = now },
            new Page { Id = premiumFaqId,     SitemapId = premiumSitemapId, Name = "FAQ",        Slug = "faq",        Title = "FAQ",        Description = "", Content = "", MetaTitle = "", MetaDescription = "", MetaKeywords = "", Status = PageStatus.Draft, SortOrder = 3, IsHomePage = false, CreatedAt = now, UpdatedAt = now }
        );

        // PageBlocks
        modelBuilder.Entity<PageBlock>().HasData(
            // Basic Home: [Header, Block 1, Footer]
            new PageBlock { Id = new Guid("50000000-0000-0000-0000-000000000001"), PageId = basicHomeId,      BlockId = headerId,  SortOrder = 1 },
            new PageBlock { Id = new Guid("50000000-0000-0000-0000-000000000002"), PageId = basicHomeId,      BlockId = block1Id,  SortOrder = 2 },
            new PageBlock { Id = new Guid("50000000-0000-0000-0000-000000000003"), PageId = basicHomeId,      BlockId = footerId,  SortOrder = 3 },
            // Basic Contact Us: [Header, Block 2, Footer]
            new PageBlock { Id = new Guid("50000000-0000-0000-0000-000000000004"), PageId = basicContactId,   BlockId = headerId,  SortOrder = 1 },
            new PageBlock { Id = new Guid("50000000-0000-0000-0000-000000000005"), PageId = basicContactId,   BlockId = block2Id,  SortOrder = 2 },
            new PageBlock { Id = new Guid("50000000-0000-0000-0000-000000000006"), PageId = basicContactId,   BlockId = footerId,  SortOrder = 3 },
            // Premium Home: [Header, Block 1, Block 3, Footer]
            new PageBlock { Id = new Guid("50000000-0000-0000-0000-000000000007"), PageId = premiumHomeId,    BlockId = headerId,  SortOrder = 1 },
            new PageBlock { Id = new Guid("50000000-0000-0000-0000-000000000008"), PageId = premiumHomeId,    BlockId = block1Id,  SortOrder = 2 },
            new PageBlock { Id = new Guid("50000000-0000-0000-0000-000000000009"), PageId = premiumHomeId,    BlockId = block3Id,  SortOrder = 3 },
            new PageBlock { Id = new Guid("50000000-0000-0000-0000-000000000010"), PageId = premiumHomeId,    BlockId = footerId,  SortOrder = 4 },
            // Premium Contact Us: [Header, Block 2, Footer]
            new PageBlock { Id = new Guid("50000000-0000-0000-0000-000000000011"), PageId = premiumContactId, BlockId = headerId,  SortOrder = 1 },
            new PageBlock { Id = new Guid("50000000-0000-0000-0000-000000000012"), PageId = premiumContactId, BlockId = block2Id,  SortOrder = 2 },
            new PageBlock { Id = new Guid("50000000-0000-0000-0000-000000000013"), PageId = premiumContactId, BlockId = footerId,  SortOrder = 3 },
            // Premium FAQ: [Header, Block 4, Footer]
            new PageBlock { Id = new Guid("50000000-0000-0000-0000-000000000014"), PageId = premiumFaqId,     BlockId = headerId,  SortOrder = 1 },
            new PageBlock { Id = new Guid("50000000-0000-0000-0000-000000000015"), PageId = premiumFaqId,     BlockId = block4Id,  SortOrder = 2 },
            new PageBlock { Id = new Guid("50000000-0000-0000-0000-000000000016"), PageId = premiumFaqId,     BlockId = footerId,  SortOrder = 3 }
        );
    }
}
