using System.Runtime.CompilerServices;
using Models;

[assembly:InternalsVisibleTo("RequestHandlerTests")]

namespace Services;

public class EntryListBuilder
{
    private readonly DataRetriever _dataRetriever;
    private readonly SemaphoreSlim _semaphore = new(1, 99);

    public EntryListBuilder(HttpClient httpClient)
    {
        var apiKey = File.ReadAllText("data/secret.txt");
        _dataRetriever = new DataRetriever(httpClient, apiKey);
    }

    public async Task<List<Entry>> BuildEntryList(bool withGarden)
    {
        var entryList = new List<Entry>();
        var uri = _dataRetriever.BuildRequestUri(withGarden);
        var firstPage = await _dataRetriever.RetrievePageData(uri, 1);
        entryList.AddRange(firstPage.Objects);

        var tasks = Enumerable.Range(2, firstPage.Paging.PageCount).Select(async page =>
        {
            Logger.Debug($"Processing page {page}");
            var pageResponse = await GetRateLimitedPageData(uri, page);
            entryList.AddRange(pageResponse.Objects);
            Logger.Debug($"Processed {entryList.Count}/{firstPage.EntryCountTotal} entries.");
        });

        await Task.WhenAll(tasks);

        if (entryList.Count != firstPage.EntryCountTotal)
        {
            Logger.Info(
                $"Unexpected number of entries returned, expected {firstPage.EntryCountTotal}, got {entryList.Count}");
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
            var rateLimitTimer = Task.Delay(new TimeSpan(0, 0, 0, 0, 601));
            response = await _dataRetriever.RetrievePageData(unpaginatedUri, pageNumber);

            //this holds the thread hostage until the rate limit timer
            await rateLimitTimer;
        }
        finally
        {
            _semaphore.Release();
        }

        return response;
    }

}