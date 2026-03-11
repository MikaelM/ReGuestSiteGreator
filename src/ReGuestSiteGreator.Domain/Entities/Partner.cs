namespace ReGuestSiteGreator.Domain.Entities;

public class Partner
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public Guid? PlanId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public User User { get; set; } = null!;
    public Plan? Plan { get; set; }
}
