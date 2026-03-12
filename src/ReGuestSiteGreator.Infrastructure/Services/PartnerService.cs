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

        var response = await _context.Pages
            .Where(p => p.Id == pageId && p.Sitemap.PlanId == partner.PlanId)
            .Select(p => new PageDetailResponse
            {
                Id = p.Id,
                Name = p.Name,
                Slug = p.Slug,
                Title = p.Title,
                Description = p.Description,
                Content = p.Content,
                MetaTitle = p.MetaTitle,
                MetaDescription = p.MetaDescription,
                MetaKeywords = p.MetaKeywords,
                Status = p.Status,
                SortOrder = p.SortOrder,
                IsHomePage = p.IsHomePage,
                ParentSlug = p.ParentSlug,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                PublishedAt = p.PublishedAt,
                Blocks = p.PageBlocks
                    .OrderBy(pb => pb.SortOrder)
                    .Select(pb => new BlockSummaryResponse { Id = pb.Block.Id, Name = pb.Block.Name })
                    .ToList()
            })
            .FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException($"Page with ID '{pageId}' was not found in your sitemap.");

        return response;
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
