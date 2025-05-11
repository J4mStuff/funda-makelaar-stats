using Newtonsoft.Json;

namespace Models;

public class Paging
{
    [JsonProperty(PropertyName = "AantalPaginas")]
    public required int PageCount { get; init; }
    
    [JsonProperty(PropertyName = "HuidigePagina")]
    public int CurrentPage { get; init; }
}