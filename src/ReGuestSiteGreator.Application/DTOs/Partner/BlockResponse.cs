using System.Text.Json;

namespace ReGuestSiteGreator.Application.DTOs.Partner;

public class BlockResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Template { get; set; } = string.Empty;
    public string Style { get; set; } = string.Empty;
    public string Script { get; set; } = string.Empty;
    public JsonElement Meta { get; set; } = JsonSerializer.Deserialize<JsonElement>("{}");
    public JsonElement DefaultData { get; set; } = JsonSerializer.Deserialize<JsonElement>("{}");
}
