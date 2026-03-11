namespace ReGuestSiteGreator.Domain.Entities;

public class PageBlock
{
    public Guid Id { get; set; }
    public Guid PageId { get; set; }
    public Guid BlockId { get; set; }
    public int SortOrder { get; set; }

    public Page Page { get; set; } = null!;
    public Block Block { get; set; } = null!;
}
