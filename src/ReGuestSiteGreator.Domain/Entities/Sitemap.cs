namespace ReGuestSiteGreator.Domain.Entities;

public class Sitemap
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid PlanId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Plan Plan { get; set; } = null!;
    public ICollection<Page> Pages { get; set; } = [];
}
