using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using ReGuestSiteGreator.Application.Common;
using ReGuestSiteGreator.Application.DTOs.Partner;
using ReGuestSiteGreator.Application.Interfaces;
using ReGuestSiteGreator.Domain.Entities;
using ReGuestSiteGreator.Infrastructure.Data;

namespace ReGuestSiteGreator.Infrastructure.Services;

public class PartnerService : IPartnerService
{
    private readonly ApplicationDbContext _context;

    public PartnerService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SitemapResponse> GetSitemapAsync(Guid userId)
    {
        var partner = await GetPartnerWithPlanAsync(userId);

        var sitemap = await _context.Sitemaps
            .Include(s => s.Pages.OrderBy(p => p.SortOrder))
            .FirstOrDefaultAsync(s => s.PlanId == partner.PlanId)
            ?? throw new KeyNotFoundException("No sitemap is configured for your plan.");

        return new SitemapResponse
        {
            Id = sitemap.Id,
            Name = sitemap.Name,
            Description = sitemap.Description,
            Pages = sitemap.Pages.Select(MapToPageSummary)
        };
    }

    public async Task<PageDetailResponse> GetPageAsync(Guid userId, Guid pageId)
    {
        var partner = await GetPartnerWithPlanAsync(userId);

        var page = await _context.Pages
            .Include(p => p.PageBlocks.OrderBy(pb => pb.SortOrder))
                .ThenInclude(pb => pb.Block)
            .Include(p => p.Sitemap)
            .FirstOrDefaultAsync(p => p.Id == pageId && p.Sitemap.PlanId == partner.PlanId)
            ?? throw new KeyNotFoundException($"Page with ID '{pageId}' was not found in your sitemap.");

        return new PageDetailResponse
        {
            Id = page.Id,
            Name = page.Name,
            Slug = page.Slug,
            Title = page.Title,
            Description = page.Description,
            Content = page.Content,
            MetaTitle = page.MetaTitle,
            MetaDescription = page.MetaDescription,
            MetaKeywords = page.MetaKeywords,
            Status = page.Status,
            SortOrder = page.SortOrder,
            IsHomePage = page.IsHomePage,
            ParentSlug = page.ParentSlug,
            CreatedAt = page.CreatedAt,
            UpdatedAt = page.UpdatedAt,
            PublishedAt = page.PublishedAt,
            Blocks = page.PageBlocks.Select(pb => MapToBlockResponse(pb.Block))
        };
    }

    public async Task<PagedResult<BlockResponse>> GetBlocksAsync(Guid userId, int page, int pageSize)
    {
        var partner = await GetPartnerWithPlanAsync(userId);

        var query = _context.PageBlocks
            .Include(pb => pb.Block)
            .Include(pb => pb.Page)
                .ThenInclude(p => p.Sitemap)
            .Where(pb => pb.Page.Sitemap.PlanId == partner.PlanId)
            .Select(pb => pb.Block)
            .Distinct();

        var totalCount = await query.CountAsync();

        var blocks = await query
            .OrderBy(b => b.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<BlockResponse>
        {
            Items = blocks.Select(MapToBlockResponse),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    private async Task<Partner> GetPartnerWithPlanAsync(Guid userId)
    {
        var partner = await _context.Partners
            .FirstOrDefaultAsync(p => p.UserId == userId)
            ?? throw new KeyNotFoundException("Partner profile not found.");

        if (partner.PlanId is null)
            throw new InvalidOperationException("No plan has been assigned to your account yet.");

        return partner;
    }

    private static PageSummaryResponse MapToPageSummary(Page page) => new()
    {
        Id = page.Id,
        Name = page.Name,
        Slug = page.Slug,
        Title = page.Title,
        Description = page.Description,
        MetaTitle = page.MetaTitle,
        MetaDescription = page.MetaDescription,
        MetaKeywords = page.MetaKeywords,
        Status = page.Status,
        SortOrder = page.SortOrder,
        IsHomePage = page.IsHomePage,
        ParentSlug = page.ParentSlug,
        PublishedAt = page.PublishedAt
    };

    private static BlockResponse MapToBlockResponse(Block block) => new()
    {
        Id = block.Id,
        Name = block.Name,
        Title = block.Title,
        Template = block.Template,
        Style = block.Style,
        Script = block.Script,
        Meta = ParseJson(block.Meta),
        DefaultData = ParseJson(block.DefaultData)
    };

    private static JsonElement ParseJson(string? json)
    {
        if (!string.IsNullOrWhiteSpace(json))
        {
            try
            {
                var element = JsonSerializer.Deserialize<JsonElement>(json);
                if (element.ValueKind == JsonValueKind.Object)
                    return element;
            }
            catch (JsonException) { }
        }

        return JsonSerializer.Deserialize<JsonElement>("{}");
    }
}
