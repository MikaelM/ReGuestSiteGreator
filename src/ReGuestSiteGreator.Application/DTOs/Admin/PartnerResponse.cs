namespace ReGuestSiteGreator.Application.DTOs.Admin;

public class PartnerResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public Guid? PlanId { get; set; }
    public string? PlanName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
