namespace Services;

public class HttpClientWrapper : IHttpClientWrapper
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("http://partnerapi.funda.nl/feeds/Aanbod.svc/")
    };

    public async Task<string> GetAndEnsureSuccessAsync(string uri)
    {
        using var response = await _httpClient.GetAsync(_httpClient.BaseAddress + uri);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}