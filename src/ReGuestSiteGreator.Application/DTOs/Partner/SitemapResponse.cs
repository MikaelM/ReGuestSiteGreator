namespace ReGuestSiteGreator.Application.DTOs.Partner;

public class SitemapResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IEnumerable<PageSummaryResponse> Pages { get; set; } = [];
}
