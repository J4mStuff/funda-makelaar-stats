using Newtonsoft.Json;

namespace Models;

public class Entry
{
    [JsonProperty(PropertyName = "Id")]
    public required Guid Id { get; init; }
    
    [JsonProperty(PropertyName = "MakelaarId")]
    public required int AgentId { get; init; }

    [JsonProperty(PropertyName = "MakelaarNaam")]
    public required string AgentName { get; init; }
    
    [JsonProperty(PropertyName = "VerkoopStatus")]
    public required string Status { get; init; }
}