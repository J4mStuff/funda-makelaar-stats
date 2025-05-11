using System.Threading.Tasks;
using Services;

namespace App;

public static class Program
{
    public static void Main()
    {
        Run().Wait();
    }

    private static async Task Run()
    {
        RequestHandler requestHandler = new();
        
        var entriesWithNoGarden = await requestHandler.MakeRequests(false);
        var topTen = AgentStatProcessor.ProcessAgents(entriesWithNoGarden);
        TablePrinter.PrintListToTable(topTen);
        
        var entriesWithGarden = await requestHandler.MakeRequests(true);
        var topTenWithGarden = AgentStatProcessor.ProcessAgents(entriesWithGarden);
        TablePrinter.PrintListToTable(topTenWithGarden);
    }
}