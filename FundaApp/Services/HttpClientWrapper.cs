namespace Services;

public class HttpClientWrapper : IHttpClientWrapper
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("http://partnerapi.funda.nl/feeds/Aanbod.svc/")
    };

    private readonly SemaphoreSlim _semaphore = new(1, 99);

    public async Task<string> MakeRateLimitedRequest(string uri)
    {
        await _semaphore.WaitAsync();
        try
        {
            //Wait 1.7s -> the rate limit is 100,
            //so if we make a request no more than avery 0.601sec we're guaranteed to stay under
            var rateLimitTimer = Task.Delay(new TimeSpan(0, 0, 0, 0, 601));
            using var response = await _httpClient.GetAsync(_httpClient.BaseAddress + uri);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            //this holds the thread hostage until the rate limit timer
            await rateLimitTimer;
            return responseString;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}