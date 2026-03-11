using ReGuestSiteGreator.Domain.Enums;

namespace ReGuestSiteGreator.Application.DTOs.Admin;

public class PlanResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public PlanType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
