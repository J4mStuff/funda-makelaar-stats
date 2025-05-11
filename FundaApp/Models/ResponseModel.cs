using Newtonsoft.Json;

namespace Models;

public class ResponseModel
{
    [JsonProperty]
    public required List<Entry> Objects { get; init; }
    
    [JsonProperty]
    public required Paging Paging { get; init; }
    
    [JsonProperty(PropertyName = "TotaalAantalObjecten")]
    public required int EntryCountTotal { get; init; }
}