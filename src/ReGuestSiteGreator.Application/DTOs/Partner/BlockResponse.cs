using System.Text.Json;

namespace ReGuestSiteGreator.Application.DTOs.Partner;

public class BlockResponse
{
    private static readonly JsonElement EmptyObject = JsonSerializer.Deserialize<JsonElement>("{}");

    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Template { get; set; } = string.Empty;
    public string Style { get; set; } = string.Empty;
    public string Script { get; set; } = string.Empty;
    public JsonElement Meta { get; set; } = EmptyObject;
    public JsonElement DefaultData { get; set; } = EmptyObject;
}
