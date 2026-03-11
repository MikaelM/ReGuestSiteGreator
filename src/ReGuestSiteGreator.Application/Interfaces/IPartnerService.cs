using ReGuestSiteGreator.Application.Common;
using ReGuestSiteGreator.Application.DTOs.Partner;

namespace ReGuestSiteGreator.Application.Interfaces;

public interface IPartnerService
{
    Task<SitemapResponse> GetSitemapAsync(Guid userId);
    Task<PageDetailResponse> GetPageAsync(Guid userId, Guid pageId);
    Task<PagedResult<BlockResponse>> GetBlocksAsync(Guid userId, int page, int pageSize);
}
