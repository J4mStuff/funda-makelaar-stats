namespace Services;

public static class StatRequestProcessor
{
    public static async Task ProcessStats(bool gardenPresent)
    {
        var httpClient = new HttpClientWrapper();
        var apiKey = await File.ReadAllTextAsync("data/secret.txt");
        var dataRetriever = new DataRetriever(httpClient, apiKey);

        var requestHandler = new EntryListBuilder(dataRetriever);

        var entriesWithNoGarden = await requestHandler.BuildEntryList(gardenPresent);
        var topTen = AgentStatProcessor.ProcessAgents(entriesWithNoGarden);
        TablePrinter.PrintListToTable(topTen);
    }
}