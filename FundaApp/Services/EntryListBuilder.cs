using System.Runtime.CompilerServices;
using Models;

[assembly:InternalsVisibleTo("RequestHandlerTests")]

namespace Services;

public class EntryListBuilder(IDataRetriever dataRetriever)
{

    public async Task<List<Entry>> BuildEntryList(bool withGarden)
    {
        var entryList = new List<Entry>();
        var uri = dataRetriever.BuildRequestUri(withGarden);
        var firstPage = await dataRetriever.RetrievePageData(uri, 1);
        entryList.AddRange(firstPage.Objects);

        var tasks = Enumerable.Range(2, firstPage.Paging.PageCount - 1).Select(async page =>
        {
            Logger.Debug($"Processing page {page}");
            var pageResponse = await dataRetriever.RetrievePageData(uri, page);
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

}