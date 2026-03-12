namespace ReGuestSiteGreator.Domain.Entities;

public class Block
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Template { get; set; } = string.Empty;
    public string Style { get; set; } = string.Empty;
    public string Script { get; set; } = string.Empty;
    /// <summary>
    /// JSON describing variables used in the template.
    /// </summary>
    public string Meta { get; set; } = "{}";
    /// <summary>
    /// JSON with default values for the template variables.
    /// </summary>
    public string DefaultData { get; set; } = "{}";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<PageBlock> PageBlocks { get; set; } = [];
}
