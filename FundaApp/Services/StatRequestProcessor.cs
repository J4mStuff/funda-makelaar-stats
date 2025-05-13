namespace Services;

public static class StatRequestProcessor
{
    public static async Task ProcessStats(IHttpClientWrapper httpClient, bool gardenPresent)
    {
        string apiKey;

        try
        {
            apiKey = await File.ReadAllTextAsync("data/secret.txt");
        }
        catch (FileNotFoundException)
        {
            Logger.Error("Secret file is missing. Please create it in FundaApp/App/data/secret.txt and try again..");
            return;
        }

        var dataRetriever = new DataRetriever(httpClient, apiKey);

        var requestHandler = new EntryListBuilder(dataRetriever);

        var entriesWithNoGarden = await requestHandler.BuildEntryList(gardenPresent);
        var topTen = AgentStatProcessor.ProcessAgents(entriesWithNoGarden);
        TablePrinter.PrintListToTable(topTen, gardenPresent);
    }
}