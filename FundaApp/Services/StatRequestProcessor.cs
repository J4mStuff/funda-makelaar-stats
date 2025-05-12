namespace Services;

public static class StatRequestProcessor
{
    public static async Task ProcessStats(bool gardenPresent)
    {
        HttpClient httpClient = new()
        {
            BaseAddress = new Uri("http://partnerapi.funda.nl/feeds/Aanbod.svc/")
        };
        
        var requestHandler = new EntryListBuilder(httpClient);

        var entriesWithNoGarden = await requestHandler.BuildEntryList(gardenPresent);
        var topTen = AgentStatProcessor.ProcessAgents(entriesWithNoGarden);
        TablePrinter.PrintListToTable(topTen);
    }
}