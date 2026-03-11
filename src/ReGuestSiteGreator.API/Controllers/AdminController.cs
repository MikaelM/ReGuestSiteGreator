using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReGuestSiteGreator.Application.Common;
using ReGuestSiteGreator.Application.DTOs.Admin;
using ReGuestSiteGreator.Application.Interfaces;

namespace ReGuestSiteGreator.API.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private const int MaxPageSize = 100;
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    /// <summary>
    /// Returns a paginated list of all partners.
    /// </summary>
    [HttpGet("partners")]
    [ProducesResponseType(typeof(PagedResult<PartnerResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPartners(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > MaxPageSize) pageSize = 20;

        var partners = await _adminService.GetPartnersAsync(page, pageSize);
        return Ok(partners);
    }

    /// <summary>
    /// Creates a new partner account.
    /// </summary>
    [HttpPost("partners")]
    [ProducesResponseType(typeof(PartnerResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreatePartner([FromBody] CreatePartnerRequest request)
    {
        try
        {
            var partner = await _adminService.CreatePartnerAsync(request);
            return CreatedAtAction(nameof(GetPartners), new { }, partner);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Returns a paginated list of all available plans.
    /// </summary>
    [HttpGet("plans")]
    [ProducesResponseType(typeof(PagedResult<PlanResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPlans(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > MaxPageSize) pageSize = 20;

        var plans = await _adminService.GetPlansAsync(page, pageSize);
        return Ok(plans);
    }

    /// <summary>
    /// Assigns a plan to a partner.
    /// </summary>
    [HttpPost("partners/{partnerId:guid}/assign-plan")]
    [ProducesResponseType(typeof(PartnerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignPlanToPartner(
        Guid partnerId,
        [FromBody] AssignPlanRequest request)
    {
        try
        {
            var partner = await _adminService.AssignPlanToPartnerAsync(partnerId, request);
            return Ok(partner);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
