using ReGuestSiteGreator.Application.DTOs.Admin;

namespace ReGuestSiteGreator.Application.Interfaces;

public interface IAdminService
{
    Task<IEnumerable<PartnerResponse>> GetPartnersAsync();
    Task<PartnerResponse> CreatePartnerAsync(CreatePartnerRequest request);
    Task<IEnumerable<PlanResponse>> GetPlansAsync();
    Task<PartnerResponse> AssignPlanToPartnerAsync(Guid partnerId, AssignPlanRequest request);
}
