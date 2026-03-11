using ReGuestSiteGreator.Application.Common;
using ReGuestSiteGreator.Application.DTOs.Admin;

namespace ReGuestSiteGreator.Application.Interfaces;

public interface IAdminService
{
    Task<PagedResult<PartnerResponse>> GetPartnersAsync(int page, int pageSize);
    Task<PartnerResponse> CreatePartnerAsync(CreatePartnerRequest request);
    Task<PagedResult<PlanResponse>> GetPlansAsync(int page, int pageSize);
    Task<PartnerResponse> AssignPlanToPartnerAsync(Guid partnerId, AssignPlanRequest request);
}
