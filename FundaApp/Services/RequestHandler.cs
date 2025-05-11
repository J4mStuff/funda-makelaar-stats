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
    private readonly SemaphoreSlim _semaphore = new(1, 99);
    private readonly JsonSerializer _jsonSerializer = JsonSerializer.Create();

    public async Task<List<Entry>> MakeRequests(bool withGarden)
    {
        var entryList = new List<Entry>();
        var key = await File.ReadAllTextAsync("data/secret.txt");
        var uri = BuildRequestString(key, withGarden);
        var firstPage = await RetrievePageData(uri, 1);
        entryList.AddRange(firstPage.Objects);

        var tasks = Enumerable.Range(2, firstPage.Paging.PageCount/10).Select(async page =>
        {
            Logger.Debug($"Processing page {page}");
            var pageResponse = await GetRateLimitedPageData(uri, page);
            entryList.AddRange(pageResponse.Objects);
            Logger.Debug($"Processed {entryList.Count}/{firstPage.EntryCountTotal} entries."); 
        });
        
        await Task.WhenAll(tasks);

        if (entryList.Count != firstPage.EntryCountTotal)
        {
            Logger.Info($"Unexpected number of entries returned, expected {firstPage.EntryCountTotal}, got {entryList.Count}");
        }
        
        return entryList;
    }
    private async Task<ResponseModel> GetRateLimitedPageData(string unpaginatedUri, int pageNumber)
    {
        ResponseModel response;
        await _semaphore.WaitAsync();
        try
        {
            //Wait 1.7s -> the rate limit is 100,
            //so if we make a request no more than avery 0.601sec we're guaranteed to stay under
            var rateLimitTimer = Task.Delay(new TimeSpan(0,0,0,0,601));
            response = await RetrievePageData(unpaginatedUri, pageNumber);

            //this holds the thread hostage until the rate limit timer
            await rateLimitTimer;
        }
        finally
        {
            _semaphore.Release();
        }
        
        return response;
    }
    private async Task<ResponseModel> RetrievePageData(string unpaginatedUri, int pageNumber)
    {
        /*
         There's a bug / "feature" where requesting page size > 25 returns 25 items per page,
         but total page count assumes page count with the requested size
         for example, there's 4583 objects total in this endpoint, asking for 500 items per page
         returns 25 items per page, but only 10 pages total in the response body.

         I understand the limitation, but it should:
         > return a bad request
         or
         > properly calculate page total
         */
        var uri = $"{unpaginatedUri}&page={pageNumber}&pagesize=25";      

        using var response = await _httpClient.GetAsync(uri);
        response.EnsureSuccessStatusCode();
        
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var responseObject = _jsonSerializer.Deserialize<ResponseModel>(new JsonTextReader(new StringReader(jsonResponse)));
        ArgumentNullException.ThrowIfNull(responseObject);
        return responseObject;
    }

    private string BuildRequestString(string apiKey, bool withGarden)
    {
        var url = _httpClient.BaseAddress + $"json/{apiKey}/?type={Constants.Purchase}&zo=/amsterdam/"; 
        return withGarden ? $"{url}{Constants.Garden}/" : url;
    }
}