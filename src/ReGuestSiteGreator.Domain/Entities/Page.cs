using ReGuestSiteGreator.Domain.Enums;

namespace ReGuestSiteGreator.Domain.Entities;

public class Page
{
    public Guid Id { get; set; }
    public Guid SitemapId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string MetaTitle { get; set; } = string.Empty;
    public string MetaDescription { get; set; } = string.Empty;
    public string MetaKeywords { get; set; } = string.Empty;
    public PageStatus Status { get; set; } = PageStatus.Draft;
    public int SortOrder { get; set; }
    public bool IsHomePage { get; set; }
    public string? ParentSlug { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }

    public Sitemap Sitemap { get; set; } = null!;
    public ICollection<PageBlock> PageBlocks { get; set; } = [];
}
