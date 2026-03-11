using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReGuestSiteGreator.Application.Common;
using ReGuestSiteGreator.Application.DTOs.Partner;
using ReGuestSiteGreator.Application.Interfaces;

namespace ReGuestSiteGreator.API.Controllers;

[ApiController]
[Route("api/partner")]
[Authorize(Roles = "Partner")]
public class PartnerController : ControllerBase
{
    private readonly IPartnerService _partnerService;

    public PartnerController(IPartnerService partnerService)
    {
        _partnerService = partnerService;
    }

    private Guid GetUserId()
    {
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub")
            ?? throw new InvalidOperationException("User ID claim not found.");
        return Guid.Parse(sub);
    }

    /// <summary>
    /// Returns the sitemap (list of pages) for the authenticated partner's plan.
    /// </summary>
    [HttpGet("sitemap")]
    [ProducesResponseType(typeof(SitemapResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSitemap()
    {
        try
        {
            var sitemap = await _partnerService.GetSitemapAsync(GetUserId());
            return Ok(sitemap);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Returns the full details of a page from the partner's sitemap.
    /// </summary>
    [HttpGet("pages/{pageId:guid}")]
    [ProducesResponseType(typeof(PageDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPage(Guid pageId)
    {
        try
        {
            var page = await _partnerService.GetPageAsync(GetUserId(), pageId);
            return Ok(page);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Returns a paginated list of all blocks used in the partner's sitemap.
    /// </summary>
    [HttpGet("blocks")]
    [ProducesResponseType(typeof(PagedResult<BlockResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBlocks(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 20;

        try
        {
            var blocks = await _partnerService.GetBlocksAsync(GetUserId(), page, pageSize);
            return Ok(blocks);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
