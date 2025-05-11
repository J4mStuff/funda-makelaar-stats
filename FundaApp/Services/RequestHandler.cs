using Config;
using Models;
using Newtonsoft.Json;

namespace Services;

public class RequestHandler
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("http://partnerapi.funda.nl/feeds/Aanbod.svc/")
    };
    
    private readonly JsonSerializer _jsonSerializer = JsonSerializer.Create();
    
    public async Task GetAsync()
    {
        var key = await File.ReadAllTextAsync("data/secret.txt");
        var uri = BuildRequestString(key, false) + "&page=1&pagesize=25";      

        using var response = await _httpClient.GetAsync(uri);

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var responseObject = _jsonSerializer.Deserialize<ResponseModel>(new JsonTextReader(new StringReader(jsonResponse)));
        Console.WriteLine($"{jsonResponse}\n");
    }

    private string BuildRequestString(string apiKey, bool withGarden)
    {
        var url = _httpClient.BaseAddress + $"json/{apiKey}/?type={Constants.Purchase}&zo=/amsterdam/"; 
        return withGarden ? $"{url}{Constants.Garden}/" : url;
    }
}