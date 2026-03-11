using ReGuestSiteGreator.Domain.Enums;

namespace ReGuestSiteGreator.Domain.Entities;

public class Plan
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public PlanType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public ICollection<Partner> Partners { get; set; } = [];
    public ICollection<Sitemap> Sitemaps { get; set; } = [];
}
